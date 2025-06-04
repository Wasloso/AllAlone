//Author: Small Hedge Games
//Updated: 13/06/2024

using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu(menuName = "Sounds SO", fileName = "Sounds SO")]
    public class SoundsSO : ScriptableObject
    {
        public SoundList[] sounds;
    }
}