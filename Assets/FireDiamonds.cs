using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Linq;
using rnd = UnityEngine.Random;
public class FireDiamonds : MonoBehaviour {
    public KMSelectable[] diamond;
    public TextMesh[] texts;
    string[][] substances = new string[][]{
        new string[] {"0", "0", "0", ""},
        new string[] {"1", "2", "0", ""},
        new string[] {"1", "0", "0", ""},
        new string[] {"2", "2", "4", ""},
        new string[] {"4", "4", "2", ""},
        new string[] {"0", "3", "3", "OX"},
        new string[] {"3", "1", "0", ""},
        new string[] {"3", "3", "2", "W"},
        new string[] {"0", "3", "1", ""},
        new string[] {"3", "2", "0", ""},
        new string[] {"0", "0", "0", "SA"},
        new string[] {"0", "4", "4","W OX"},
        new string[] {"0", "1", "0", "OX"},
        new string[] {"1", "1", "0", ""},
        new string[] {"4", "2", "0", "SA"},
        new string[] {"2", "3", "0", ""},
        new string[] {"4", "1", "0", ""},
        new string[] {"0", "3", "0", ""},
        new string[] {"4", "2", "0", ""},
        new string[] {"0", "3", "2", "W"},
        new string[] {"0", "0", "1", ""},
        new string[] {"1", "3", "0", ""},
        new string[] {"0", "2", "1", "OX"},
        new string[] {"0", "4", "0", "" }
     };
    int[][] sequences = new int[][]{
        new int[] {0,2,3,3,3}, 
        new int[] {1,1,2,1,1}, 
        new int[] {0,1,2,1,0}, 
        new int[] {2,2,2,2,2}, 
        new int[] {1,2,0,2,0}, 
        new int[] {0,0,1,3,1}, 
        new int[] {2,2,0,0,1}, 
        new int[] {2,2,2,2,0}, 
        new int[] {3,1,3,1,3}, 
        new int[] {0,0,3,0,1}, 
        new int[] {1,1,2,3,2}, 
        new int[] {1,0,3,2,1}, 
        new int[] {1,2,3,1,3}, 
        new int[] {2,2,2,2,3}, 
        new int[] {0,1,3,2,0}, 
        new int[] {2,1,1,3,1},
        new int[] {0,1,2,3,3}, 
        new int[] {3,0,2,2,2}, 
        new int[] {0,0,2,2,2}, 
        new int[] {2,1,0,3,1}, 
        new int[] {3,3,0,3,0}, 
        new int[] {3,1,1,1,3},
        new int[] {3,1,3,0,2},
        new int[] {0,3,3,3,3}
    };
    string[] substanceNames = new string[] {"Table Salt", "Antifreeze", "Glucose", "Nitroglycerin", "Hydrogen Cyanide", "Hydrogen Peroxide", "Gasoline", "Sodium", "Drain Cleaner", "Ethanol", "Helium Gas", "Fluorine Gas", "Potassium Nitrate", "Put another chemical here", "Methane", "Acetic Acid", "Butane", "Iodine", "Propane", "Sulfuric Acid", "Baking Soda", "Sodium Hypochlorite (Bleach)", "Hydroflouric Acid"};
    string[] colorsLogging = new string[] { "Red", "Blue", "Yellow", "White" };
    string[] usedSubstance;
    int[] usedSequence;
    bool[] breaks = new bool[4];
    string indicators;
    int pressCounter;
    int stageCounter;
    bool invalid;
    bool running;
    public KMBombModule module;
    public KMBombInfo bomb;
    public KMAudio sound;
    int moduleId;
    static int moduleIdCounter = 1;
    bool solved;

    void Awake() {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable cell in diamond) {
            KMSelectable i = cell;
            i.OnInteract += delegate { pressDiamond(i); return false; };
        }
    }

    void Start () {
        indicators = bomb.GetIndicators().Join();
        CalcStage();
        for (int i = 0; i < 4; i++)
        {
            texts[i].text = usedSubstance[i];
        }
	}

    void CalcStage() {
        usedSubstance = substances[rnd.Range(0, 23)];
        Debug.LogFormat("[Fire Diamonds #{0}] The subtance on the module is {1}.", moduleId, substanceNames[Array.IndexOf(substances, usedSubstance)]);
        usedSequence = sequences[Array.IndexOf(substances, usedSubstance)];
        Debug.LogFormat("[Fire Diamonds #{0}] The sequence before applying tranformations is {1}, {2}, {3}, {4}, {5}.", moduleId, colorsLogging[usedSequence[0]], colorsLogging[usedSequence[1]], colorsLogging[usedSequence[2]], colorsLogging[usedSequence[3]], colorsLogging[usedSequence[4]]);
        int index = 0;
        foreach (int i in usedSequence) {
            switch (i)
            {
                case 0:
                switch (bomb.GetBatteryCount()) { 
                        case 0:
                        case 1:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 1;
                                    break;
                                case 1:
                                    usedSequence[index] = 3;
                                    break;
                                case 2:
                                    usedSequence[index] = 0;
                                    break;
                            }
                            break;
                        case 2:
                            switch (stageCounter)
                            {
                             case 0:
                            usedSequence[index] = 0;
                            break;
                            case 1:
                            usedSequence[index] = 2;
                            break;
                            case 2:
                            usedSequence[index] = 1;
                            break;
                    }
                    break;
                        default:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 2;
                                    break;
                                case 1:
                                    usedSequence[index] = 0;
                                    break;
                                case 2:
                                    usedSequence[index] = 0;
                                    break;
                            }
                            break;
                    }
                    break;
                case 1:
                    switch (bomb.GetSerialNumberNumbers().Count())
                    {
                        case 2:                       
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 3;
                                    break;
                                case 1:
                                    usedSequence[index] = 2;
                                    break;
                                case 2:
                                    usedSequence[index] = 3;
                                    break;
                            }
                            break;
                        case 3:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 2;
                                    break;
                                case 1:
                                    usedSequence[index] = 0;
                                    break;
                                case 2:
                                    usedSequence[index] = 1;
                                    break;
                            }
                            break;
                        case 4:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 0;
                                    break;
                                case 1:
                                    usedSequence[index] = 1;
                                    break;
                                case 2:
                                    usedSequence[index] = 2;
                                    break;
                            }
                            break;
                    }
                    break;
                case 2: 
                    switch (bomb.GetSerialNumber().Count(x => x == 'N' || x == 'F' || x == 'P' || x == 'A' || x == '7' || x == '0' || x == '4'))
                    {
                        case 0:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 1;
                                    break;
                                case 1:
                                    usedSequence[index] = 2;
                                    break;
                                case 2:
                                    usedSequence[index] = 0;
                                    break;
                            }
                            break;
                        case 1:
                        case 2:   
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 0;
                                    break;
                                case 1:
                                    usedSequence[index] = 0;
                                    break;
                                case 2:
                                    usedSequence[index] = 1;
                                    break;
                            }
                            break;
                        default:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 2;
                                    break;
                                case 1:
                                    usedSequence[index] = 0;
                                    break;
                                case 2:
                                    usedSequence[index] = 1;
                                    break;
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (indicators.Count(x => x != 'A' && x != 'E' && x != 'I' && x != 'O' && x != 'U'))
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 3;
                                    break;
                                case 1:
                                    usedSequence[index] = 1;
                                    break;
                                case 2:
                                    usedSequence[index] = 0;
                                    break;
                            }
                            break;
                        case 4:
                        case 5:
                        case 6:
                                    usedSequence[index] = 2;
                                    break;
                        default:
                            switch (stageCounter)
                            {
                                case 0:
                                    usedSequence[index] = 0;
                                    break;
                                case 1:
                                    usedSequence[index] = 3;
                                    break;
                                case 2:
                                    usedSequence[index] = 0;
                                    break;
                            }
                            break;
                    }
                            break;
            }
            index++;
        }
        Debug.LogFormat("[Fire Diamonds #{0}] The sequence after applying tranformations is {1}, {2}, {3}, {4}, {5}.", moduleId, colorsLogging[usedSequence[0]], colorsLogging[usedSequence[1]], colorsLogging[usedSequence[2]], colorsLogging[usedSequence[3]], colorsLogging[usedSequence[4]]);
    }

    void pressDiamond(KMSelectable cell)
    {
        if (!solved)
        {
            if (!running)
            {
                Debug.LogFormat("[Fire Diamonds #{0}] You pressed {1}.", moduleId, colorsLogging[Array.IndexOf(diamond, cell)]);
                if (usedSequence[pressCounter] != Array.IndexOf(diamond, cell))
                {
                    invalid = true;
                }
                pressCounter++;
                if (pressCounter == 5)
                {
                    pressCounter = 0;
                    if (!invalid)
                    {
                        stageCounter++;
                        if (stageCounter == 3)
                        {
                            Debug.LogFormat("[Fire Diamonds #{0}] That was correct. Module solved.", moduleId);
                            usedSubstance = new string[] { "S", "O", "L", "VED" };
                            StartCoroutine(unstableController());
                        }
                        else
                        {
                            Debug.LogFormat("[Fire Diamonds #{0}] That was correct. Advancing to the next stage.", moduleId);
                            CalcStage();
                            StartCoroutine(unstableController());
                        }
                    }
                    else
                    {
                        Debug.LogFormat("[Fire Diamonds #{0}] That was incorrect. Strike!", moduleId);
                        module.HandleStrike();
                        CalcStage();
                        StartCoroutine(unstableController());
                    }
                }
            }
            }
        }
 

    IEnumerator unstableController()
    {
        breaks = new bool[] { false, false, false, false };
        running = true;
        StartCoroutine(unstableCycle(texts[0]));
        StartCoroutine(unstableCycle(texts[1]));
        StartCoroutine(unstableCycle(texts[2]));
        StartCoroutine(unstableCycle(texts[3]));
        yield return new WaitForSeconds(4.25f);
        texts[0].text = usedSubstance[0];
        breaks[0] = true;
        StopCoroutine(unstableCycle(texts[0]));
        yield return new WaitForSeconds(.25f);
        texts[1].text = usedSubstance[1];
        breaks[1] = true;
        StopCoroutine(unstableCycle(texts[1]));
        yield return new WaitForSeconds(.25f);
        texts[2].text = usedSubstance[2];
        breaks[2] = true;
        StopCoroutine(unstableCycle(texts[2]));
        yield return new WaitForSeconds(.25f);
        texts[3].text = usedSubstance[3];
        breaks[3] = true;
        StopCoroutine(unstableCycle(texts[3]));
        running = false;
        if (stageCounter == 3)
        {
            module.HandlePass();
            solved = true;
        }
    }

    IEnumerator unstableCycle(TextMesh text)
    {
        int textIndex = 0;
        string[] textOptions;
        if(Array.IndexOf(texts,text) == 3)
        {
            textOptions = new string[] { "", "W", "OX", "SA", "W OX" };
        }
        else
        {
            textOptions = new string[] { "0", "1", "2", "3", "4" };
        }
        while (true)
        {
            if (breaks[Array.IndexOf(texts, text)])
            {
                break;
            }
            text.text = textOptions[textIndex];

            ++textIndex;

            if (textIndex >= textOptions.Count())
                textIndex = 0;

            yield return new WaitForSeconds(.01f);

        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Use '!{0} red/blue/yellow/white' to press the corresponding square. Can be chained.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string[] commandArray = command.Split(' ');
        string[] validcmds = new string[] { "red", "blue", "yellow", "white" };
        {
            for (int i = 0; i < commandArray.Length; i++)
            {
                if (!validcmds.Contains(commandArray[i]))
                {
                    yield return "sendtochaterror Invalid command.";
                    yield break;
                }
            }
            for (int i = 0; i < commandArray.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (commandArray[i] == validcmds[j])
                    {
                        yield return null;
                        diamond[j].OnInteract();
                    }
                }
            }
            yield break;
        }
    }
  /*  IEnumerator TwitchHandleForcedSolve()
    {
        while (!solved)
        {
            foreach (int i in usedSequence)
            {
                yield return null;
                diamond[i].OnInteract();
            }
        }
    } */
}

