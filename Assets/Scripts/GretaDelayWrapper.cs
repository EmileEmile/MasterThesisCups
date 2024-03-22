using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GretaDelayWrapper : MonoBehaviour
{
    [System.Serializable]
    public struct AnimationCommand
    {
        public string name;
        public float length;

        public AnimationCommand(string name, float length)
        {
            this.name = name;
            this.length = length;
        }

        public AnimationCommand AddPrefix(string prefix)
        {
            return new AnimationCommand(prefix + name, length);
        }
    }

    private GretaCharacterAnimator gretaAnimator;

    private LinkedList<AnimationCommand> waitingAnimations = new LinkedList<AnimationCommand>();

    public float timeSinceLastPlay = 0;
    private float waitTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        gretaAnimator = GetComponent<GretaCharacterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gretaAnimator.agentPlaying! && waitingAnimations.Count > 0 && timeSinceLastPlay > waitTime)
        {

            Debug.Log("Playing Anim: " + waitingAnimations.First.Value.name);
            gretaAnimator.agentPlaying = true;
            AnimationCommand nextCommand = waitingAnimations.First.Value;
            waitingAnimations.RemoveFirst();
            gretaAnimator.PlayAgentAnimation(nextCommand.name);
            timeSinceLastPlay = 0;
            waitTime = nextCommand.length;
        }
        else
        {
            timeSinceLastPlay += Time.deltaTime;
        }
    }

    public void AddGretaAnimationWithMinDelay(float sec, AnimationCommand command)
    {
        StartCoroutine(AnimationWithMinDelay(sec, command));
    }

    public void AddGretaAnimationSeriesWithMinDelay(float sec, List<AnimationCommand> commands)
    {
        StartCoroutine(AnimationsWithMinDelay(sec, commands));
    }

    public void InteruptGretaAnimation(AnimationCommand command)
    {
        waitTime = 0;
        waitingAnimations.AddFirst(command);
    }

    public void ClearAnimationQueue()
    {
        waitingAnimations.Clear();
    }

    private IEnumerator AnimationWithMinDelay(float sec, AnimationCommand command)
    {
        yield return new WaitForSeconds(sec);

        waitingAnimations.AddLast(command);
    }
    private IEnumerator AnimationsWithMinDelay(float sec, List<AnimationCommand> commands)
    {
        yield return new WaitForSeconds(sec);

        for (int i = 0; i < commands.Count; i++)
        {
            waitingAnimations.AddLast(commands[i]);
            Debug.Log(commands[i].name);
        } 
    }
}
