using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;

public class FrozenHeart : RepeatSkill
{
  [SerializeField] private GameObject[] _spinner = new GameObject[6];
  [SerializeField] private GameObject[] _spinnerFinal = new GameObject[6];
  
  private void Awake()
  {
    SkillType = ESkillType.FrozenHeart;
    gameObject.SetActive(false);
    SetActiveSpinner(false);
  }
  
  public override void ActivateSkill()
  {
    gameObject.SetActive(true);
    SetActiveSpinner(true);
    ActivateSpinner();
  }

  protected override void OnChangedSkillData()
  {
    SetActiveSpinner(true);
    SetFrozenHeart();
  }

  private void SetActiveSpinner(bool isActive)
  {
    if (Level == 6)
    {
      foreach (GameObject spinner in _spinnerFinal)
        spinner.SetActive(isActive);

      foreach (GameObject spinner in _spinner)
        spinner.SetActive(false);
    }
    else
    {
      foreach (GameObject spinner in _spinner)
        spinner.SetActive(isActive);

      foreach (GameObject spinner in _spinnerFinal)
        spinner.SetActive(false);
    }
  }
  
  private void SetFrozenHeart()
  {
    transform.localPosition = Vector3.zero;
    if (Level == 6)
    {
      for (int i = 0; i < _spinnerFinal.Length; i++)
      {
        if (i < SkillData.numProjectiles)
        {
          _spinnerFinal[i].SetActive(true);
          float degree = 360f / SkillData.numProjectiles * i;
          _spinnerFinal[i].transform.localPosition = Quaternion.Euler(0f, 0f, degree) * Vector3.up * SkillData.projRange;
          _spinnerFinal[i].transform.localScale = Vector3.one * SkillData.scaleMultiplier;
        }
        else
        {
          _spinnerFinal[i].SetActive(false);
        }
      }
    }
    else
    {
      for (int i = 0; i < _spinner.Length; i++)
      {
        if (i < SkillData.numProjectiles)
        {
          _spinner[i].SetActive(true);
          float degree = 360f / SkillData.numProjectiles * i;
          _spinner[i].transform.localPosition = Quaternion.Euler(0f, 0f, degree) * Vector3.up * SkillData.projRange;
          _spinner[i].transform.localScale = Vector3.one * SkillData.scaleMultiplier;
        }
        else
        {
          _spinner[i].SetActive(false);
        }
      }
    }
  }
  
  private void ActivateSpinner()
  {
    SetFrozenHeart();
    Sequence enableSequence = DOTween.Sequence();
    gameObject.SetActive(true);

    float speed = SkillData.rotateSpeed * SkillData.duration;
    Tween scale = transform.DOScale(1, 0.2f);
    Tween rotate = transform.DORotate(new Vector3(0, 0, speed), SkillData.duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

    Tween scale2 = transform.DOScale(0, 1f);
    Tween rotate2 = transform.DORotate(new Vector3(0, 0, speed * 1), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

    enableSequence.Append(scale).Join(rotate)
      .Append(scale2).Join(rotate2)
      .InsertCallback(SkillData.duration , () => gameObject.SetActive(false))
      .AppendInterval(SkillData.coolTime)
      .AppendCallback(() => ActivateSpinner());
  }
  
  protected override void DoSkillJob()
  {
    throw new System.NotImplementedException();
  }
}
