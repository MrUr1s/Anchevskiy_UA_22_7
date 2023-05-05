using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Ziggurat;

using UnityEngine.EventSystems;
[RequireComponent(typeof(UnitEnvironment))]
public class NPC : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private NPC _target;
    [SerializeField]
    private bool _isAttack;
    [SerializeField]
    private bool _isMoving;
    [SerializeField]
    private int _hp = 0;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private UnitEnvironment _unitEnvironment;
    [SerializeField]
    private NPCSetting _nPCSetting;
    public NPC Target => _target;
    public NPCSetting NPCSetting => _nPCSetting;
    public Rigidbody Rigidbody => _rigidbody;


    private static string _fastAttack = "Strong";
    private static string _strongAttack = "Fast";
    private static string _Die = "Die";

    public int Hp
    {
        get => _hp;
        private set
        {
            _hp = value;
            if (Hp < 0)
            {
                Die();
            }
        }
    }

    public bool IsAttack => _isAttack;
    private UnitEnvironment _environment;

    private Rigidbody _rigidbody;
    [SerializeField]
    private bool _endAniamtionAttack;
   

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _unitEnvironment = GetComponent<UnitEnvironment>();
        _unitEnvironment.OnEndAnimation += _unitEnvironment_OnEndAnimation;
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _environment = GetComponent<UnitEnvironment>();
        if (NPCSetting == null)
            _nPCSetting = new NPCSetting();
        SetSetting(NPCSetting);
    }

    private void _unitEnvironment_OnEndAnimation(object sender, EventArgs e)
    {
        if (sender is string)
            if((sender as string).Equals("StrongAttack")|| (sender as string).Equals("FastAttack"))
        _endAniamtionAttack = true;
    }

    private void Die()
    {
        _environment.StartAnimation(_Die);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(Move());
        StopCoroutine(Attack());
    }
    private void OnEnable()
    {
        _endAniamtionAttack=_isMoving = true;
        StartCoroutine(Move());
        StartCoroutine(Attack());
    }
    public void SetSetting(NPCSetting setting)
    {
        _nPCSetting = (NPCSetting)setting.Clone();
        _skinnedMeshRenderer.material = NPCSetting.Material;
        _hp = _nPCSetting.MaxHP;
        _isAttack = false;
    }

    public void SearchTarget()
    {
        _target = PoolNPC.Instance.GetUnitEnemyTeam(this);
    }
    private IEnumerator Attack()
    {
        while (true)
        {
            if (IsAttack && !_isMoving && _target != null && _endAniamtionAttack)
            {
                var crit = 1;
                if (_nPCSetting.ChanceMiss >= UnityEngine.Random.Range(0, 100))
                    crit = 0;
                if (_nPCSetting.ChanceCrit >= UnityEngine.Random.Range(0, 100))
                    crit = 2;
                if (_nPCSetting.Probability <= UnityEngine.Random.Range(0, 100))
                {
                    _target.Hp -= _nPCSetting.FastHit * crit;
                    _environment.StartAnimation(_fastAttack);
                }
                else
                {
                    _target.Hp -= _nPCSetting.StrongHit * crit;
                    _environment.StartAnimation(_strongAttack);
                }

                if (!_target.gameObject.activeSelf)
                {
                    _target = null;
                }
                _isMoving = true;
                _isAttack = false;
                _endAniamtionAttack = false;
            }
            yield return null;
        }
    }
    private IEnumerator Move()
    {
        while (true)
        {
            if (_target != null && _isMoving && !_isAttack)
            {
                Vector3 target = _target.transform.position;
                transform.LookAt(target);
                var targetPosition = target;
                var desVelocity = targetPosition - transform.position;
                var sqrLength = desVelocity.sqrMagnitude;
                if (sqrLength < 3)
                {
                    sqrLength = 0;
                    _isAttack = true;
                    _isMoving = false;
                }
                else
                    _isAttack = false;
                
                desVelocity = desVelocity.normalized * sqrLength;

                var steering = desVelocity - GetVelocity();
                var velocity = Vector3.ClampMagnitude(GetVelocity() + steering, NPCSetting.Speed);

                SetVelocity(velocity);
            }
            yield return null;
        }
    }

    

    private void SetVelocity(Vector3 velocity)
    {
        _unitEnvironment.Moving(velocity.normalized.sqrMagnitude);

        Rigidbody.velocity = velocity;
    }

    private Vector3 GetVelocity()
    {
        return new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LogManager.Instance?.SaveLog("OnPointerClick to NPC");
        UIManager.Instance.GateUI.Close();
       // UIManager.Instance.NpcUI.Open();
        UIManager.Instance.NpcUI.Load(this);
    }
}
