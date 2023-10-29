using Godot;
using System;

public partial class Gun : Area2D 
{

    public float firerateRoundsPerSecond;
    public int damage;
    int max_ammo;
    int ammo;

    public Gun(float _firerateRoundsPerSecond, int _damage, int _max_amo)
    {
        this.firerateRoundsPerSecond = _firerateRoundsPerSecond;
        this.damage = _damage;
        this.max_ammo = _max_amo;
        this.ammo = _max_amo;
    }



}