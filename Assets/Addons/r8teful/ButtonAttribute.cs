using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute {
        // This is the field for the button text
        public string buttonText;

        // This is the constructor that takes the button text as a parameter
        public ButtonAttribute(string buttonText) {
            this.buttonText = buttonText;
        }
    }