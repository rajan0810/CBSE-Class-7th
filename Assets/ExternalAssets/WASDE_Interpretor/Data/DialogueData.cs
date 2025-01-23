using UnityEngine;
using System.Collections.Generic;

public enum VarOperators
{
    ADD, SUB, MUL, DIV, EXP, LOG10, LOG, LESSTHAN, GREATERTHAN, EQUAL, LESSEQUAL, GREATEREQUAL
}
public struct OptionData
{
    public VarConstOperation[] constCheck;
    public VarVarOperation[] varCheck;
    public string title;
    public int id;
}
public struct VarConstOperation
{
    public string varName;
    public VarOperators op;
    public float num;
}
public struct VarVarOperation
{
    public string varName;
    public VarOperators op;
    public string var2Name;
}

public class DialogueTree
{
    public Dictionary<string, float> variables;
    public Character[] chars;
    public DialogueData[] dialogues;
}

public struct Character
{
    public int id;
    public string Name;
}
public class DialogueData
{
    public int id;
    public string title;
    public string line;
    public OptionData[] options = new OptionData[0];
    public VarConstOperation[] variableConstantOperations;
    public VarVarOperation[] variableVariableOperations;

    //left to right
    public int[] charIDs = new int[3];

    //0,1,2 for left, center, right
    public int charCurrentlySpeaking;
    public DialogueData() {      
        id = -1;
        title = "Untitled Monologue";
        line = "";
        options = new OptionData[0];
        variableConstantOperations = new VarConstOperation[0];
        variableVariableOperations = new VarVarOperation[0];
        charIDs = new int[3];
        charCurrentlySpeaking = -1;
    }
    public DialogueData(DialogueData dd) {
        
        id = dd.id;
        title = dd.title;
        line = dd.line;
        options = dd.options ;
        variableConstantOperations = dd.variableConstantOperations;
        variableVariableOperations = dd.variableVariableOperations;
        charIDs = dd.charIDs;
        charCurrentlySpeaking = dd.charCurrentlySpeaking;
    }

}
