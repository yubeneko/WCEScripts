using UnityEngine;
using Valve.VR;
using UniRx;
using UniRx.Triggers;
using System;

namespace WCE.Players
{
    public class ViveControllerInputProvider : MonoBehaviour, IInputProvider
    {
        private readonly Subject<Unit> _onDischarge = new Subject<Unit>();
        private readonly Subject<string> _onAttributeChange = new Subject<string>();
        private readonly ReactiveProperty<bool> _onPause = new ReactiveProperty<bool>();
        public IObservable<Unit> OnDischarge => _onDischarge;
        public IObservable<string> OnAttributeChange => _onAttributeChange;
        public IReadOnlyReactiveProperty<bool> OnPause => _onPause;

        [SerializeField]
        private SteamVR_ActionSet wceAction;
        [SerializeField]
        private SteamVR_Input_Sources hand = SteamVR_Input_Sources.RightHand;
        [SerializeField]
        private SteamVR_Action_Pose pose = SteamVR_Actions._default.Pose;
        [SerializeField]
        private SteamVR_Action_Boolean isPauseButtonPushed;
        [SerializeField]
        private SteamVR_Action_Boolean isTrackpadPushed;
        [SerializeField]
        private SteamVR_Action_Vector2 trackpadValue;

        void Start ()
        {
            wceAction.Activate();
            DischargeObserve();
            PauseButtonObserve();
            AttributeChangeObserve();
        }

        private bool isHandMoving = false;
        private ReactiveProperty<int> stopJudgeCounter = new ReactiveProperty<int>();

        void DischargeObserve ()
        {
            var controllerVelocityObserver = pose
               .ObserveEveryValueChanged(p => p.GetVelocity(hand).sqrMagnitude);

            controllerVelocityObserver
                .Where(x => x > 35f)
                .Subscribe(_ => isHandMoving = true);

            controllerVelocityObserver
                .Where(_ => isHandMoving)
                .Where(x => x < 1.0f)
                .Subscribe(_ => stopJudgeCounter.Value++);

            stopJudgeCounter.Where(x => x == 10)
                   .Subscribe(_ =>
                   {
                       _onDischarge.OnNext(Unit.Default);
                       stopJudgeCounter.Value = 0;
                       isHandMoving = false;
                   });
        }

        void PauseButtonObserve ()
        {
            this.UpdateAsObservable()
                .Select(_ => isPauseButtonPushed.GetState(SteamVR_Input_Sources.Any))
                .DistinctUntilChanged()
                .Subscribe(x => _onPause.Value = x);
        }

        void AttributeChangeObserve ()
        {
            var OnTrackpadClickedXAxisObserver = this.UpdateAsObservable()
                .Select(_ => isTrackpadPushed.GetState(hand))
                .DistinctUntilChanged()
                .Where(x => x)
                .Select(_ => trackpadValue.GetAxis(hand).x);

            OnTrackpadClickedXAxisObserver
                .Where(x => x >= 0)
                .Subscribe(_ => _onAttributeChange.OnNext("right"));

            OnTrackpadClickedXAxisObserver
                .Where(x => x < 0)
                .Subscribe(_ => _onAttributeChange.OnNext("left"));
        }
    }
}