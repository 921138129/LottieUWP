using System;

namespace LottieUWP
{
    public class ValueAnimator : Animator
    {
        protected ValueAnimator()
        {
            Interpolator = new AccelerateDecelerateInterpolator();
        }

        public class ValueAnimatorUpdateEventArgs : EventArgs
        {
            public ValueAnimator Animation { get; }

            public ValueAnimatorUpdateEventArgs(ValueAnimator animation)
            {
                Animation = animation;
            }
        }

        public event EventHandler<ValueAnimatorUpdateEventArgs> Update;

        private float _floatValue1;
        private float _floatValue2;
        private float _animatedValue;
        private IInterpolator _interpolator;

        public IInterpolator Interpolator
        {
            get => _interpolator;
            set
            {
                if(value == null)
                    value = new LinearInterpolator();
                _interpolator = value;
            }
        }

        public float AnimatedFraction { get; private set; }

        public float AnimatedValue
        {
            get => _animatedValue;
            private set
            {
                _animatedValue = value;
                OnAnimationUpdate();
            }
        }

        public override void Start()
        {
            AnimatedFraction = Interpolator.GetInterpolation(0);
            AnimatedValue = MathExt.Lerp(_floatValue1, _floatValue2, AnimatedFraction);

            base.Start();
        }

        void OnAnimationUpdate()
        {
            Update?.Invoke(this, new ValueAnimatorUpdateEventArgs(this));
        }

        protected override void TimerCallback(object state)
        {
            base.TimerCallback(state);

            AnimatedFraction = Interpolator.GetInterpolation(Progress);
            AnimatedValue = MathExt.Lerp(_floatValue1, _floatValue2, AnimatedFraction);
        }

        public void SetFloatValues(float floatValue1, float floatValue2)
        {
            _floatValue1 = floatValue1;
            _floatValue2 = floatValue2;
        }

        public static ValueAnimator OfFloat(float floatValue1, float floatValue2)
        {
            return new ValueAnimator
            {
                _floatValue1 = floatValue1,
                _floatValue2 = floatValue2
            };
        }
    }
}