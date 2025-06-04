using UnityEngine;
public interface ILight
{
	float Range { get; }
	Color LightColor { get; }
	float Intensity { get; }
}