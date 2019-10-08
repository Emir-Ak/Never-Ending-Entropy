using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class AZAnim
{
    public static bool TypeWritingIsFinished;
    public static bool TypeWritingListIsFinished;
    public static bool AnimatingScaleIsFinished;
    public static bool AnimatingPositionIsFinished;
    public static bool MenuAnimatingIsFinished;

    /// <summary>
    /// Creates effect on a text that makes it look like it is being typed in (NEEDS StartCoroutine() TO RUN!)
    /// </summary>
    /// <param name="text">Text to put effect on</param>
    /// <param name="result">String that is required to be seen as an end result of the text</param>
    /// <param name="time">Time it would take for the effect to take place : FixedUpdate if less then 0.01</param>
    /// <param name="character">Character which won't be typed in, but will put a following text on to a new line</param>
    /// <param name="relative">Would text to take effect on be relative to existing text (or should original text be deleted)?</param>
    /// <param name="delay">Time on seconds before effect will take place</param>
    public static IEnumerator TypeWrite(Text text, string result, float time = 1f, char character = ';', bool relative = false, float delay = 0f)
    {
        #region LOGIC
        TypeWritingIsFinished = false;

        //If text to typewrite is not relative to original... (If it is, characters would be added onto existing text)
        if (relative == false)
        {
            //Clean the original text
            text.text = "";
        }

        //Delay before typewriting
        yield return new WaitForSeconds(delay);

        //Learn the time to spend for each letter
        float _timePerCharacter = time / result.Length;

        //For every character in the text required...
        foreach (char i in result)
        {
            //Will put the following text on to new line
            if (i == character)
            {
                text.text += "\n";
                continue;
            }

            text.text += i;
            if (time >= 0.01)
            {
                yield return new WaitForSeconds(_timePerCharacter);
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }

        TypeWritingIsFinished = true;
        #endregion
    }
    
    /// <summary>
    /// Creates effect on list of texts that makes it look like texts are being typed in one after another (NEEDS StartCoroutine() TO RUN!)
    /// </summary>
    /// <param name="texts">List of texts that you want to be typed in</param>
    /// <param name="time">Time it would take for the effect to take place</param>
    /// <param name="character">Character that would act as new line, and won't be seen</param>
    /// <param name="relative">Should the text be added onto existing one?</param>
    /// <param name="delay">Delay in time before coroutine starts running</param>
    public static IEnumerator TypeWriteList(List<Text> texts, float time = 10f, char character = ';', bool relative = false, float delay = 0f)
    {
        #region LOGIC
        TypeWritingListIsFinished = false;

        //Delay before typewriting List
        yield return new WaitForSeconds(delay);

        //Calculate time for each text in an list
        float _timePerText = time / texts.Count;

        //Foreach text in an list...
        foreach (Text text in texts)
        {
            //Typewrite whatever was already present in the text;
            TypeWrite(text, text.text, _timePerText,character, relative);

            //Wait till typing is finished
            yield return new WaitWhile(()=> !TypeWritingIsFinished);
        }

        TypeWritingListIsFinished = true;
        #endregion
    }

    /// <summary>
    /// Animates the whole menu, scales in the objects and/or typewrited the text (NEEDS StartCoroutine() TO RUN!)
    /// </summary>
    /// <param name="instance">Instance of monobehaviour class, just put "this" keyword - We need to know when to run sub-coroutines ;-)</param>
    /// <param name="texts">List of texts to animate</param>
    /// <param name="objects">List of objects to animate</param>
    /// <param name="typeWriteFirst">Should the texts be typed in firsed?</param>
    /// <param name="timeForTexts">Time for text animation</param>
    /// <param name="scalingSpeed">Value that will be passed to additional vector, how fast it will be scaling</param>
    /// <param name="delay">Time before animation starts</param>
    public static IEnumerator AnimateMenu(MonoBehaviour instance, List<Text> texts = null, List<GameObject> objects = null, float time = 10f,bool typeWriteFirst = false, float delay = 0f)
    {
        #region LOGIC
        MenuAnimatingIsFinished = false;

        //Calculates time per text
        time = time / ((texts != null ? texts.Count : 0f) + (objects != null ? objects.Count : 0f));

        TypeWritingIsFinished = false;
        AnimatingScaleIsFinished = false;
        //Lists to keep original values
        List<Vector3> originalScales = new List<Vector3>();
        List<string> originalTexts = new List<string>();

        //Save original scales and delete previous values
        if (objects != null)
        {
            foreach (GameObject obj in objects)
            {
                originalScales.Add(obj.transform.localScale);
                obj.transform.localScale = Vector3.zero;
            }
        }

        //Save original strings from texts delete previous values
        if (texts != null)
        {
            foreach (Text text in texts)
            {
                originalTexts.Add(text.text);
                text.text = string.Empty;
            }
        }

        //Wait before animation starts
        yield return new WaitForSeconds(delay);

        if (typeWriteFirst == false)
        {

            //Animate scale for all objects
            if (objects != null)
            {
                foreach (GameObject obj in objects)
                {
                    instance.StartCoroutine(AnimateScale(obj, originalScales[objects.IndexOf(obj)], time));
                    yield return new WaitUntil(() => AnimatingScaleIsFinished);
                }
            }

            //Type in all texts
            if (texts != null)
            { 
                foreach (Text text in texts)
                {
                    instance.StartCoroutine(TypeWrite(text, originalTexts[texts.IndexOf(text)], time));
                    yield return new WaitUntil(() => TypeWritingIsFinished);
                }
            }
        }

        else
        {
            //Type in all texts
            if (texts != null)
            {
                foreach (Text text in texts)
                {
                   instance.StartCoroutine(TypeWrite(text, originalTexts[texts.IndexOf(text)], time));
                    yield return new WaitUntil(() => TypeWritingIsFinished);
                }
            }

            //Animate scale for all objects
            if (objects != null)
            {
                foreach (GameObject obj in objects)
                {
                    instance.StartCoroutine(AnimateScale(obj, originalScales[objects.IndexOf(obj)], time));
                    yield return new WaitUntil(() => AnimatingScaleIsFinished);
                }
            }
        }

        MenuAnimatingIsFinished = true;
        #endregion
    }

    /// <summary>
    /// Animates localScale of the object, from nothing to chosen scale (NEEDS StartCoroutine() TO RUN!)
    /// </summary>
    /// <param name="obj">Object to animate</param>
    /// <param name="targetScale">Vector3, to which the object will be scaled</param>
    /// <param name="time">Tie for animation</param>
    /// <param name="delay">Delay before animation starts</param>
    public static IEnumerator AnimateScale(GameObject obj, Vector3 targetScale, float time, float delay = 0f)
    {
        #region LOGIC
        AnimatingScaleIsFinished = false;

        //Set localScale to 0
        obj.transform.localScale = new Vector3(0.1f,0.1f,0.1f);

        //Wait before starting
        yield return new WaitForSeconds(delay);

        float timeCountDown = time;

        while (timeCountDown > 0.0f)
        {
            timeCountDown -= Time.fixedDeltaTime;

            obj.transform.localScale = Vector3.Lerp(targetScale, obj.transform.localScale, timeCountDown / time);
            yield return new WaitForFixedUpdate();
        }

        AnimatingScaleIsFinished = true;
        #endregion
    }

    /// <summary>
    /// Animates localScale of the object, from nothing to chosen scale (NEEDS StartCoroutine() TO RUN!)
    /// </summary>
    /// <param name="obj">Object to animate</param>
    /// <param name="targetScale">Vector3, to which the object will be scaled</param>
    /// <param name="speed">Self explanatory...</param>
    /// <param name="delay">Delay before animation starts</param>
    public static IEnumerator AnimatePosition(GameObject obj, Vector3 targetPoition, float speed, float delay = 0f)
    {
        #region LOGIC
        AnimatingPositionIsFinished = false;


        //Wait before starting
        yield return new WaitForSeconds(delay);
        while (obj.transform.position != targetPoition)
        {
            Debug.Log("animating pos");
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPoition, Time.deltaTime * speed); ;
            yield return new WaitForFixedUpdate();
        }

        AnimatingPositionIsFinished = true;
        #endregion
    }
    /// <summary>
    /// Limits and returns the limited string by the given parameters! (Not animation but whatever, it still helps a lot)
    /// </summary>
    /// <param name="stringToLimit">String that you want to be returned limited</param>
    /// <param name="caseMode">0: Lower case, 1: Upper case</param>
    /// <param name="characterLimitationMode">0: Numerical, 1: Alphabetical, 2: Alphanumerical,</param>
    /// <param name="noSpaces">Should any spaces be present?</param>
    /// <returns>Limited string</returns>
    public static string LimitString (string stringToLimit, int caseMode = -1, int characterLimitationMode = -1, bool noSpaces = false)
    {
        
        #region LOGIC
        string limitedText = stringToLimit;

        //Upper or Lower case
        if (characterLimitationMode != 0) {
            if (caseMode == 0)
                limitedText = limitedText.ToLower();
            else if (caseMode == 1)
                limitedText = limitedText.ToUpper();
        }

        //Spaces deleted
        if (noSpaces == true)
        {
           string tmpString = "";
           foreach(char i in limitedText)
            {
                if (i != ' ')
                {
                    tmpString += i;
                }
            }
           limitedText = tmpString;
        }

        //Alphanumerical limitation
        Regex rgx = new Regex("");
        if (characterLimitationMode == 0)
        {
            rgx = new Regex("[^0-9 ]");
        }
        else if (characterLimitationMode == 1)
        {
            rgx = new Regex("[^a-zA-z ]"); 
        }
        else if (characterLimitationMode == 2)
        {
            rgx = new Regex("[^a-zA-z0-9 ]");
        }
        limitedText = rgx.Replace(limitedText, "");

        return limitedText;
        #endregion
    }
}


