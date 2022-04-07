using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는데 사용할 타입을 선언
    public enum State { Ready, Empty, Reloading }
    
    // 현재 총의 상태
    public State state { get; private set; }

    // 탄알이 발사될 위치
    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect; // 총구 화염
    public ParticleSystem shellEjectEffect; // 탄피 배출

    // 탄알 궤적을 그리기 위한 렌더러
    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer; // 총소리 재생기

    //public AudioClip shotClip; // 발사 소리
    //public AudioClip reloadClip; // 재장전 소리
    //public float damage = 25; // 공격력
    //public int ammoRemain = 100; // 남은 전체 탄알
    //public int magCapacity = 25; // 탄창 용량

    // 총의 현재 데이터
    public GunData gunData;

    private float fireDistance = 50f; // 사정거리
    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄창에 남아 있는 탄알

    //public float timeBetFire = 0.12f;
    //public float reloadTime = 1.8f;
    // 총을 마지막으로 발사한 시점
    private float lastFireTime;

    private void Awake()
    {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두 개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 전체 예비 탄알 양을 초기화
        ammoRemain = gunData.startAmmoRemain;
        // 현재 탄창을 가득 채우기
        magAmmo = gunData.magCapacity;
        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        // 마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;
    }

    public void Fire() // 발사 시도
    {
        // 현재 상태가 발사 가능한 상태
        // && 마지막 총 발사 시점에서 timeBetFire 이상의 시간이 지남
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // 마지막 총 발사 시점 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot();
        }
    }

    private void Shot() // 실제 발사 처리
    {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 탄알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, 
            out hit, fireDistance))
        {
            // 레이가 어떤 물체와 충돌한 경우

            // 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            IDamageble target = hit.collider.GetComponent<IDamageble>();

            // 상대방으로부터 IDamageable 오브젝트를 가져오는데 성공했다면
            if(target != null)
            {
                // 상대방의 OnDamage 함수를 실행시켜 상대방에 대미지 주기
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 탄알이 최대 사정거리까지 날아갔을 때의 위치를
            // 충돌 위치로 사용
            hitPosition = fireTransform.position + 
                fireTransform.forward * fireDistance;
        }

        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));

        // 남은 탄알 수를 -1
        magAmmo--;
        if(magAmmo <= 0)
        {
            // 탄창에 남은 탄알이 없다면 총의 현재 상태를 Empty로 갱신
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitposition)
    {
        // 총구 화염 효과 재생
        muzzleFlashEffect.Play();
        // 탄피 배출 효과 재생
        shellEjectEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitposition);
        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    public bool Reload() // 재장전 시도
    {
        return false;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;

        // 재장전 소요 시간만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}
