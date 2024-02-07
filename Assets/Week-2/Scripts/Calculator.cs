using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    //The text box object storing the calculator results
    public TextMeshProUGUI text;

    //I made this property to directly access the text string of the text box object
    private string ResultString
    {
        get => text.text;
        set => text.text = value;
    }

    //Stores the first number in a calcuation
    public float prevInput;

    //Signals to empty the text box when you start inputting the second number in a calculation (or clear the calculator)
    public bool clearPrevInput;

    //Keeps track of what kind of calculation to do (+, -, x, /)
    private EquationType equationType;

    private void Start()
    {
        Clear();
    }

    //Used to input numbers into the calculator
    public void AddInput(string input)
    {
        //Empties the text box if we start inputting the second number in a calculation (or if it's our first input)
        if (clearPrevInput)
        {
            ResultString = string.Empty;
            clearPrevInput = false;
        }

        //Appends whatever number was pressed to the end of the text box
        ResultString += input;
    }

    //Called by + button
    public void SetEquationAsAdd()
    {
        prevInput = float.Parse(ResultString); //Convert and store the first input as a float
        clearPrevInput = true; //Signal we're about to start inputting the second number
        equationType = EquationType.ADD;
    }

    //Called by - button
    public void SetEquationAsSubtract()
    {
        prevInput = float.Parse(ResultString); //Convert and store the first input as a float
        clearPrevInput = true; //Signal we're about to start inputting the second number
        equationType = EquationType.SUBTRACT;
    }

    //Called by x button
    public void SetEquationAsMultiply()
    {
        prevInput = float.Parse(ResultString); //Convert and store the first input as a float
        clearPrevInput = true; //Signal we're about to start inputting the second number
        equationType = EquationType.MULTIPLY;
    }

    //Called by / button
    public void SetEquationAsDivide()
    {
        prevInput = float.Parse(ResultString); //Convert and store the first input as a float
        clearPrevInput = true; //Signal we're about to start inputting the second number
        equationType = EquationType.DIVIDE;
    }

    //Adds the first and second number and updates the text to the result
    public void Add()
    {
        float result = prevInput + float.Parse(ResultString);
        ResultString = result.ToString();
    }

    //Subtracts the first and second number and updates the text to the result
    public void Subtract()
    {
        float result = prevInput - float.Parse(ResultString);
        ResultString = result.ToString();
    }

    //Multiplies the first and second number and updates the text to the result
    public void Multiply()
    {
        float result = prevInput * float.Parse(ResultString);
        ResultString = result.ToString();
    }

    //Divides the first and second number and updates the text to the result
    public void Divide()
    {
        float result = prevInput / float.Parse(ResultString);
        ResultString = result.ToString();
    }

    public void Clear()
    {
        ResultString = "0"; //Reset the text box
        clearPrevInput = true; //Signal we're entering a new number
        prevInput = 0; //Reset the "first" number

        //Indicates no kind of calculation has been selected
        equationType = EquationType.None;
    }

    //Called by = button
    public void Calculate()
    {
        //Does the correct calculation based on what button was pressed
        if (equationType == EquationType.ADD) Add();
        if (equationType == EquationType.SUBTRACT) Subtract();
        if (equationType == EquationType.MULTIPLY) Multiply();
        if (equationType == EquationType.DIVIDE) Divide();
    }

    //Holds the different kinds of calculations
    public enum EquationType
    {
        None = 0,
        ADD = 1,
        SUBTRACT = 2,
        MULTIPLY = 3,
        DIVIDE = 4
    }
}
