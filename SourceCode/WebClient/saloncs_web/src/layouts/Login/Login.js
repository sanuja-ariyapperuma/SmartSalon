import React,{ useState,useRef } from "react";
import { useHistory  } from "react-router-dom";
import Notification from "components/Notification/Notification";
import {
    Button,
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    Input,
    Form,
    Row,
    Col,
    UncontrolledTooltip
  } from "reactstrap";

const Login = () => {

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const history = useHistory();
    const childRef = useRef();

    const onLoginClick = (event) =>{
        event.preventDefault();
        LoginCall(username,password)
    }

    const LoginCall = () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                username: username,
                password: password
        })
        }

        fetch(
            'https://localhost:7209/auth/login', 
            requestOptions)
            .then((res) => res.json())
            .then((post) => {
                console.log(post);
                if(post.success){
                    history.push("/admin/dashboard");
                }else{
                    childRef.current.error("Username or Password Error")
                }
            })
            .catch((err) => {
                console.log(err.message);
            });
        
    }

    return(<>
        <div className="content">
            <Notification  ref={childRef} />
            <Row>
                <Col xs="12" sm="10" md="8" lg="4" className="mx-auto align-self-center ">
                    <Card className="card-chart text-center ">
                        <CardHeader>
                            <Col className="text-left" sm="">
                                <CardTitle tag="h2">Login</CardTitle>
                            </Col>
                        </CardHeader>
                        <CardBody>
                            <Form>
                                <Row>
                                    <Col>
                                        <Input className="my-2" placeholder="Username" type="text" onChange={e => setUsername(e.target.value)}/>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <Input className="my-2" placeholder="Password" type="password" onChange={e => setPassword(e.target.value)}/>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                    <Button className="btn-fill" color="primary" type="submit" onClick={onLoginClick}>
                                        Login
                                    </Button>
                                    </Col>
                                </Row>
                            </Form>
                        </CardBody>
                    </Card>
                </Col>
            </Row>
        </div>
    </>)
}

export default Login;