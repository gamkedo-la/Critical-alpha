using UnityEngine;
using System.Collections;

public interface IDamageable
{
	void Damage(int damage);

    bool IsDead { get; }
}
