import React, { forwardRef, useImperativeHandle } from "react";
import NotificationAlert from "react-notification-alert";

const Notification = forwardRef((props, ref) =>{
  const notificationAlertRef = React.useRef(null);
  useImperativeHandle(ref, () => ({
    error(message){
      const options = {
        place: "tc",
        message: (
            <div>
              <div>
                {message}
              </div>
            </div>
          ),
        type: "danger",
        icon: "tim-icons icon-bell-55",
        autoDismiss: 5
    }
    notificationAlertRef.current.notificationAlert(options);
  }
  }));
    return (<>
        <div className="react-notification-alert-container">
            <NotificationAlert ref={notificationAlertRef} />
        </div>
    </>)
})

export default Notification;