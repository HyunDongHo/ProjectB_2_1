using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerRotationCreate : EnemyRotationCreate
{
    public Vector2 xRandom;
    public Vector2 yRandom;
    public Vector2 zRandom;

    public override Quaternion GetRotation()
    {
        return Quaternion.Euler(Random.Range(xRandom.x, xRandom.y), Random.Range(yRandom.x, yRandom.y), Random.Range(zRandom.x, zRandom.y));        
    }
}
