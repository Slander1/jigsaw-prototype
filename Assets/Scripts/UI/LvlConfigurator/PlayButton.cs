using TMPro;
using UnityEngine;

namespace UI.LvlConfigurator
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Init(string playText)
        {
            text.text = playText;
        }
    }
}