import React from 'react'
import { useState } from 'react';
import './login.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function Login() {
    const [email, setEmail] = useState('');
    const [readersCode, setReadersCode] = useState('');
    const navigate = useNavigate();

    const submit = async (e) => {
        e.preventDefault();

        const response = await axios.post('https://localhost:5001/api/account/login', {
            Id: 0,
            Email: email,
            ReadersCode: readersCode,
            Firstname: "",
            Lastname: "",
            Class: "",
            Role: false,
            Token:""
        });

        const content = await response.data;
        console.log(content);

        if(content != null)
        {
            localStorage.setItem("user", JSON.stringify(content));
            navigate("/books");
            window.location.reload();
        }
    }

   
    return (
        <div className='justify-content-center wholelogin'>
            <section className="h-100 login-form">
                <div className="container py-5 h-100">
                    <div className="row d-flex justify-content-center align-items-center h-100">
                        <div className="col-xl-10">
                            <div className="card rounded-3 text-black">
                                <div className="row g-0">
                                    <div className="col-lg-6 leftside">
                                        <div className="card-body p-md-5 mx-md-4">

                                            <div className="text-center">
                                                <h4 className="mt-1 mb-5 pb-1">Bejelentkezés</h4>
                                            </div>

                                            <form onSubmit={submit}>
                                                <p className='center' >Kérjük jelentkezzen be a fiókjába!</p>

                                                <div className="form-group mb-4">
                                                    <input type="email" id="form2Example11" className="form-control"
                                                        placeholder="E-mail cím" required
                                                        onChange={e => setEmail(e.target.value)} />
                                                </div>

                                                <div className="form-group mb-4">
                                                    <input type="text" id="form2Example22" className="form-control" placeholder='Olvasójegy száma'
                                                        required
                                                        onChange={e => setReadersCode(e.target.value)} />
                                                </div>
                                                <div className="text-center pt-1 mb-5 pb-1">
                                                    <button className='btn btn-outline-dark' type="submit">Bejelentkezés
                                                    </button>
                                                </div>

                                            </form>

                                        </div>
                                    </div>
                                    <div className="col-lg-6 d-flex align-items-center rightside_bg">
                                        <div className="text-white px-3 py-4 p-md-5 mx-md-4">
                                            <h4 className="mb-4"> </h4>
                                            <p className="small mb-0"></p>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    )
};

export default Login