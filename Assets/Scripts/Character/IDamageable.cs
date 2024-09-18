using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillData;

public interface IDamageable
{
    void Hurt(float damage,ICharacterController source,HitData hitData);
}
