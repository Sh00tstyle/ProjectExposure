﻿using UnityEngine;
using UnityEngine.UI;

namespace CurvedVRKeyboard {

    [SelectionBase]
    public class KeyboardStatus : KeyboardComponent {

        //-----------SET IN UNITY --------------
        [SerializeField]
        public string output;
        [SerializeField]
        public int maxOutputLength;
        [SerializeField]
        public GameObject targetGameObject;


        //----CurrentKeysStatus----
        [SerializeField]
        public Component typeHolder;
        [SerializeField]
        public bool isReflectionPossible;
        private KeyboardItem[] keys;
        private bool areLettersActive = true;
        private bool isLowercase = true;
        private const char BLANKSPACE = ' ';
        private const string TEXT = "text";
        private Text textComponent;

        public void Awake() {
            textComponent = targetGameObject.GetComponent<Text>();
        }


        /// <summary>
        /// Handles click on keyboarditem
        /// </summary>
        /// <param name="clicked">keyboard item clicked</param>
        public void HandleClick(KeyboardItem clicked) {
            string value = clicked.GetValue();

            if (value.Equals(QEH) || value.Equals(ABC)) { // special signs pressed
                ChangeSpecialLetters();
            } else if (value.Equals(UP) || value.Equals(LOW)) { // upper/lower case pressed
                LowerUpperKeys();
            } else if (value.Equals(SPACE)) {
                TypeKey(BLANKSPACE);
            } else if (value.Equals(BACK)) {
                BackspaceKey();
            } else if (value.Equals(DONE)) {
                //do nothing
            } else {// Normal letter
                TypeKey(value[0]);
            }
        }

        /// <summary>
        /// Displays special signs
        /// </summary>
        private void ChangeSpecialLetters() {
            KeyLetterEnum ToDisplay = areLettersActive ? KeyLetterEnum.NonLetters : KeyLetterEnum.LowerCase;
            areLettersActive = !areLettersActive;
            isLowercase = true;
            for (int i = 0; i < keys.Length; i++) {
                keys[i].SetKeyText(ToDisplay);
            }
        }

        /// <summary>
        /// Changes between lower and upper keys
        /// </summary>
        private void LowerUpperKeys() {
            KeyLetterEnum ToDisplay = isLowercase ? KeyLetterEnum.UpperCase : KeyLetterEnum.LowerCase;
            isLowercase = !isLowercase;
            for (int i = 0; i < keys.Length - 2; i++) {
                keys[i].SetKeyText(ToDisplay);
            }
        }

        private void BackspaceKey() {
            if (output.Length >= 1) {
                textComponent.text = textComponent.text.Remove(output.Length - 1, 1);
                output = textComponent.text;
            }
        }

        private void TypeKey(char key) {
            textComponent.text = textComponent.text + key.ToString();
            output = textComponent.text;

        }

        public void SetKeys(KeyboardItem[] keys) {
            this.keys = keys;
        }

        public void setOutput(ref string stringRef) {
            output = stringRef;
        }
    }
}
