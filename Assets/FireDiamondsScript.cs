using System;
using System.Collections;
using UnityEngine;
using KModkit;
using System.Linq;
using rnd = UnityEngine.Random;

public class FireDiamondsScript : MonoBehaviour {

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
    string[] substanceNames = new string[] {"Table Salt", "Antifreeze", "Glucose", "Nitroglycerin", "Hydrogen Cyanide", "Hydrogen Peroxide", "Gasoline", "Sodium", "Drain Cleaner", "Ethanol", "Helium Gas", "Fluorine Gas", "Potassium Nitrate", "Sulfur", "Methane", "Acetic Acid", "Butane", "Iodine", "Propane", "Sulfuric Acid", "Baking Soda", "Ammonia", "Sodium Hypochlorite (Bleach)", "Hydroflouric Acid"};
    string[] colorsLogging = new string[] { "Red", "Blue", "Yellow", "White" };
    string[] usedSubstance;
    int[] usedSequence;
    int[] presses = new int[5];
    string indicators;
    int pressCounter;
    int stageCounter;
    bool invalid;
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
        indicators = bomb.GetIndicators().Join("");
        CalcStage();
        ReformatLooks();
	}

    void CalcStage() {
        usedSubstance = substances[rnd.Range(0, 24)];
        Debug.LogFormat("[Fire Diamonds #{0}] <Stage {2}> The substance on the module is {1}.", moduleId, substanceNames[Array.IndexOf(substances, usedSubstance)], stageCounter + 1);
        usedSequence = new int[5];
        for (int i = 0; i < 5; i++)
            usedSequence[i] = sequences[Array.IndexOf(substances, usedSubstance)][i];
        Debug.LogFormat("[Fire Diamonds #{0}] <Stage {6}> The sequence before applying tranformations is {1}, {2}, {3}, {4}, {5}.", moduleId, colorsLogging[usedSequence[0]], colorsLogging[usedSequence[1]], colorsLogging[usedSequence[2]], colorsLogging[usedSequence[3]], colorsLogging[usedSequence[4]], stageCounter + 1);
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
        Debug.LogFormat("[Fire Diamonds #{0}] <Stage {6}> The sequence after applying tranformations is {1}, {2}, {3}, {4}, {5}.", moduleId, colorsLogging[usedSequence[0]], colorsLogging[usedSequence[1]], colorsLogging[usedSequence[2]], colorsLogging[usedSequence[3]], colorsLogging[usedSequence[4]], stageCounter + 1);
    }

    void pressDiamond(KMSelectable cell)
    {
        if (!solved)
        {
            if (usedSequence[pressCounter] != Array.IndexOf(diamond, cell))
            {
                invalid = true;
            }
            cell.AddInteractionPunch();
            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, cell.transform);
            presses[pressCounter] = Array.IndexOf(diamond, cell);
            pressCounter++;
            if (pressCounter == 5)
            {
                pressCounter = 0;
                Debug.LogFormat("[Fire Diamonds #{0}] You pressed {1}, {2}, {3}, {4}, {5}.", moduleId, colorsLogging[presses[0]], colorsLogging[presses[1]], colorsLogging[presses[2]], colorsLogging[presses[3]], colorsLogging[presses[4]]);
                presses = new int[5];
                if (!invalid)
                {
                    stageCounter++;
                    sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                    if (stageCounter == 3)
                    {
                        Debug.LogFormat("[Fire Diamonds #{0}] That was correct. Module solved.", moduleId);
                        for (int i = 0; i < 4; i++)
                            texts[i].text = "";
                        module.HandlePass();
                        solved = true;
                    }
                    else
                    {
                        Debug.LogFormat("[Fire Diamonds #{0}] That was correct. Advancing to the next stage.", moduleId);
                        CalcStage();
                        ReformatLooks();
                    }
                }
                else
                {
                    invalid = false;
                    Debug.LogFormat("[Fire Diamonds #{0}] That was incorrect. Strike! Resetting Stage.", moduleId);
                    module.HandleStrike();
                    CalcStage();
                    ReformatLooks();
                }
            }
        }
    }
 
    private void ReformatLooks()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < 3 && usedSubstance[i].Equals("2"))
            {
                texts[i].transform.localScale = new Vector3(3.8f, 4.2f, 1f);
                texts[i].transform.localPosition = new Vector3(0.01f, -0.03f, -0.51f);
                texts[i].characterSize = 0.023f;
            }
            else if (i < 3)
            {
                texts[i].transform.localScale = new Vector3(4f, 4f, 1f);
                texts[i].transform.localPosition = new Vector3(0f, 0f, -0.51f);
                texts[i].characterSize = 0.025f;
            }
            else if (i == 3 && usedSubstance[i].Equals("OX"))
            {
                texts[i].transform.localScale = new Vector3(2.1f, 2.1f, 1f);
                texts[i].transform.localPosition = new Vector3(0.02f, 0.02f, -0.51f);
                texts[i].characterSize = 0.025f;
            }
            else if (i == 3 && usedSubstance[i].Equals("SA"))
            {
                texts[i].transform.localScale = new Vector3(2.1f, 2.1f, 1f);
                texts[i].transform.localPosition = new Vector3(0f, 0f, -0.51f);
                texts[i].characterSize = 0.025f;
            }
            else if (i == 3 && usedSubstance[i].Equals("W"))
            {
                texts[i].transform.localScale = new Vector3(2f, 2f, 1f);
                texts[i].transform.localPosition = new Vector3(0f, 0f, -0.51f);
                texts[i].characterSize = 0.03f;
            }
            else if (i == 3 && usedSubstance[i].Equals("W OX"))
            {
                texts[i].transform.localScale = new Vector3(0.9f, 1.9f, 1f);
                texts[i].transform.localPosition = new Vector3(0f, 0f, -0.51f);
                texts[i].characterSize = 0.03f;
            }
            texts[i].text = usedSubstance[i];
        }
    }

    #pragma warning disable 414
    private string TwitchHelpMessage = "Use '!{0} red/blue/yellow/white' to press the corresponding square (can be chained or abbreviated). Use '!{0} reset' to clear all inputs.";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string[] commandArray = command.Split(' ');
        string[] validcmds = new string[] { "red", "blue", "yellow", "white", "r", "b", "y", "w" };
        if (commandArray[0] == "reset")
        {
            yield return null;
            invalid = false;
            pressCounter = 0;
            presses = new int[5];
        }
        else
        {
            for (int i = 0; i < commandArray.Length; i++)
            {
                if (!validcmds.Contains(commandArray[i]))
                {
                    yield return "sendtochaterror Invalid command.";
                    yield break;
                }
            }
            yield return null;
            for (int i = 0; i < commandArray.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (commandArray[i] == validcmds[j])
                    {
                        if (j > 3)
                            diamond[j - 4].OnInteract();
                        else
                            diamond[j].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }
        yield break;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (invalid)
        {
            for (int i = 0; i < 4; i++)
                texts[i].text = "";
            module.HandlePass();
            solved = true;
            yield break;
        }
        int start1 = stageCounter;
        int start2 = pressCounter;
        for (int i = start1; i < 3; i++)
        {
            for (int j = start2; j < 5; j++)
            {
                diamond[usedSequence[j]].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
            if (start2 != 0)
                start2 = 0;
        }
    }
}

