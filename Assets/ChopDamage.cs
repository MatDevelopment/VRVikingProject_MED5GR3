using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDamage : MonoBehaviour
{
private HealthSystem healthsystem;
private void Awake()
{
    healthsystem = new HealthSystem(100);

}

}
