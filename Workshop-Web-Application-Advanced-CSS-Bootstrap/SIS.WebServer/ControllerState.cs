using SIS.MvcFramework.Validation;

namespace SIS.MvcFramework
{
    public class ControllerState : IControllerState
    {
        public ModelStateDictionary ModelState { get; set; }

        public ControllerState()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.ModelState = new ModelStateDictionary();
        }

        public void Initialize(Controller controller)
        {
            this.ModelState = controller.ModelState;
        }

        public void SetState(Controller controller)
        {
            controller.ModelState = this.ModelState;
        }
    }
}
