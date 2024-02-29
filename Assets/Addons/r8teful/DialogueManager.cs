using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : StaticInstance<DialogueManager> {
    public bool NoDialoguePlaying => eventStack.Count == 0;
    public DialogueEventSO CurrentEvent => NoDialoguePlaying ? null : eventStack.Peek();

    [SerializeField]
    private DialogueText dialogueText = default;
    public Stack<DialogueEventSO> eventStack = new Stack<DialogueEventSO>();
    private List<DialogueEventSO> playedEvents = new List<DialogueEventSO>();
    public Speaker DialogueSpeaker { get; set; }
    public enum Speaker {
        PhonePerson,
        DisplayText,
        Outside
    }
    private void Start() {
        dialogueText.DisplayCharacter += OnDisplayCharacter;
        dialogueText.Clear();
        //dialogueText.CompletedLine += OnMessageEnded;
        //dialogueText.EmotionChange += OnEmotionChange;
    }

    public void AddDialogueEventToStack(DialogueEventSO dialogueEvent) {
        if (!playedEvents.Contains(dialogueEvent)) {
            if (eventStack.Count == 0 || eventStack.Peek().interruptBehaviour != DialogueEventSO.InterruptBehaviour.Uninterruptable) {
                StartCoroutine(PlayDialogueEvent(dialogueEvent));
            }
        }
    }

    private IEnumerator PlayDialogueEvent(DialogueEventSO dialogueEvent) {
        playedEvents.Add(dialogueEvent);

        eventStack.Push(dialogueEvent);
        dialogueText.Clear();

        bool continueToNextLine = true;
        for (int i = 0; i < dialogueEvent.lines.Count; i++) {
            if (eventStack.Peek() != dialogueEvent) {
                yield return new WaitWhile(() => eventStack.Peek() != dialogueEvent);
                if(dialogueEvent.interruptBehaviour.Equals(DialogueEventSO.InterruptBehaviour.Skip)) {
                    continueToNextLine = false;
                }
            }
            var line = dialogueEvent.lines[i];
            if (continueToNextLine) {
                dialogueText.PlayMessage(line);
                yield return new WaitUntil(() => !dialogueText.PlayingMessage || eventStack.Peek() != dialogueEvent);
                yield return new WaitForSeconds(0.15f);
            }
        }

        yield return new WaitWhile(() => eventStack.Peek() != dialogueEvent);
        eventStack.Pop();
        if (eventStack.Count == 0) {
            yield return new WaitForSeconds(0.75f);
            if (eventStack.Count == 0) {
                dialogueText.Clear();
            }
        }
    }
    private void OnDisplayCharacter(string message, int index) {
        //string[] vowels = { "Am", "Im", "Om", "Em", "Um" };
        //int randomIndex = Random.Range(0, vowels.Length);
        //string selectedVowel = vowels[randomIndex];
        // TODO hardcored player position
        if (DialogueSpeaker.Equals(Speaker.PhonePerson)) {
            AudioController.Instance.PlaySound3D("Voice", new Vector3(0, 2.5f, -27), pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.VerySmall, dialogueText.VoicePitchAdjust),
                repetition: new AudioParams.Repetition(0.075f * 2), randomization: new AudioParams.Randomization(true), distortion: new AudioParams.Distortion(false, true));
        } else if (DialogueSpeaker.Equals(Speaker.DisplayText)) {
            AudioController.Instance.PlaySound2D("Pad", pitch: new AudioParams.Pitch(AudioParams.Pitch.Variation.Small, dialogueText.VoicePitchAdjust),
               repetition: new AudioParams.Repetition(0.075f * 2));
        }
    }

    /*
    private void OnEmotionChange(Emotion emotion) {
        eyes.SetEmotion(emotion);
        mouth.SetEmotion(emotion);
    }

    
    private void OnMessageEnded() {
        mouth.SetOpen(false);
    }*/
}
