import React from 'react'
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import "./Profile.css";

function Profile() {

  const [isLoggedIn, setLoggedIn] = useState(false);
  const [user, setUser] = useState(Object);
  const [checkouts, setCheckouts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const _user = localStorage.getItem("user");
    if (_user == null) {
      setLoggedIn(false);
      navigate("/login");
    }
    else {
      const obj = JSON.parse(_user);
      setLoggedIn(true);
      setUser(JSON.parse(_user));


      
      let checkoutlist;
      (async () => {
        try {
          const response = await axios.get("https://localhost:5001/api/checkOut/GetByMember/" + obj.id);
          checkoutlist = await response.data;

          for (let i = 0; i < checkoutlist.length; i++) {
            checkoutlist[i].Book = await GetBookInfo(checkoutlist[i].bookId);
          }
        }
        catch (err) {
          console.log(err);
        }
        finally {
          setCheckouts(checkoutlist);
        }
      })();
    }
  }, [GetBookInfo, navigate]);

  async function GetBookInfo(stockId) {
    const response = await axios.get("https://localhost:5001/api/BookStock/GetByStockID/" + stockId);
    let obj = JSON.parse(JSON.stringify(response.data));
    obj.author = await GetAuthorsName(obj.id);
    return obj;
  }

  async function GetAuthorsName(bookId) {
    const authorList = await axios.get("https://localhost:5001/api/Book/GetAuthors/" + bookId);
    let author = "";

    for await (const instance of authorList.data) {
      author += instance + ", ";
    }
    return author.slice(0, -2);
  }

  function CanExtendDays(date1) {
    let d1 = new Date(date1);
    let d2 = new Date(Date.now());
    let diff = Math.abs(d1.getTime() - d2.getTime());
    let diffDays = Math.ceil(diff / (1000 * 3600 * 24));
    console.log(diffDays);
    return diffDays < 3;
  }

  async function ExtendDays(checkout) {
    let d1 = new Date(checkout.checkInDate);
    d1.setDate(d1.getDate() + 4);
    checkout.checkInDate = d1.toISOString().split('T')[0];

    const res = await axios.put('https://localhost:5001/api/CheckOut', checkout);
    alert(res.data);
    window.location.reload();
  }

  return (
    <div className="">
      {isLoggedIn ? (
        <div className="ppage">
          <div className="container">
            <div className="row">
              <div className="col-sm">
                <div className="card">
                  <div className="card-body persdata">
                    <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" className="bi bi-file-earmark-text" viewBox="0 0 16 16">
                      <path d="M5.5 7a.5.5 0 0 0 0 1h5a.5.5 0 0 0 0-1h-5zM5 9.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5zm0 2a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2a.5.5 0 0 1-.5-.5z" />
                      <path d="M9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V4.5L9.5 0zm0 1v2A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5z" />
                    </svg>
                    <h5 className="card-title ptitle">Személyes adatok</h5> <br />
                    <h6 className="card-text"><b>Név: </b> {user.firstname} {user.lastname}</h6>
                    <h6 className="card-text"><b>Osztály: </b> {user.class}</h6>
                    <h6 className="card-text"><b>Email: </b> {user.email}</h6>
                    <h6 className="card-text"><b>Olvasójegy száma: </b> {user.readersCode}</h6>
                  </div>
                </div>
                <br />
              </div>
              <div className="col-sm-8">
                    <h5><button className='border-0'><b>Kölcsönzések </b></button><button className='border-0'><b>Foglalások </b></button> </h5>
                    {checkouts.map((checkOut) => (

                      <div className="card" key={checkOut.id}>
                        <div className="card-body d-flex">
                          <img src={`https://localhost:5001/Img/${checkOut.Book.link}`} alt="" height={150} />
                          <div className="card-body cdata">
                            <b>{checkOut.Book.title}</b>
                            <br />
                            <b>{checkOut.Book.author} <br /></b>
                            <b>Kikölcsönzés dátuma: </b> {checkOut.checkOutDate.slice(0, -9)}
                            <br /> 
                            <b>Állapot: </b>{checkOut.isReturned ? "Visszahozva" : "Kikölcsönözve"}
                            {!checkOut.isReturned && (<div><b>Határidő: </b>{checkOut.checkInDate.slice(0, -9)}</div>)}
                            {checkOut.isReturned && (<div><b>Visszahozás dátuma: </b>{checkOut.returnDate.slice(0, -9)}</div>)}
                            <br />
                            {!checkOut.isReturned && CanExtendDays(checkOut.checkInDate.slice(0, -9)) && (<button className='btn btn-outline-dark pbutton' onClick={() => { ExtendDays(checkOut) }} >Meghosszabítás 3 nappal</button>)}
                            <br />
                          </div>
                          <div>
                            
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
            </div>
          </div>
        </div>) : (
        <div>
        </div>)}
    </div>
  )
}

export default Profile