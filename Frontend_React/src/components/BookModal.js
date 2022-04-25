import React from 'react'
import "./BookModal.css";
import { useEffect } from 'react';

function BookModal({ closeModal, currentBook }) {

    /* Ne mocorogjon az oldal mikor meg van nyitva a modal */
    useEffect(() => {
        if (closeModal) {
            const width = document.body.clientWidth;
            document.body.style.overflow = "hidden";
            document.body.style.width = `${width}px`;
        } else {
            document.body.style.overflow = "visible";
            document.body.style.width = `auto`;
        }

        return () => {
            document.body.style.overflow = "visible";
            document.body.style.width = `auto`;
        };
    }, [closeModal]);

    return (
        <div className="modal in" id="exampleModal" tabindex="-1" role="dialog" style={{ display: "flex" }}>

            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title btitle" id="exampleModalLabel">{currentBook.title}</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" onClick={() => { closeModal(false) }}></button>
                    </div>
                    <div className="modal-body">
                        <div className='d-flex'>
                            <img src={`https://localhost:5001/Img/${currentBook.link}`} alt="" height={300} className="bimg" />
                            <div className='bdata'>
                            <b>Szerző:<br /></b>{currentBook.author}
                                <br /> 
                            <b> Kategória:<br /></b>{currentBook.category}
                                <br /> <br/>
                            <b>Kiadó:<br /></b>{currentBook.publisher}
                                <br />
                            <b>Kiadási év:<br /></b>{currentBook.publishYear}
                                <br />
                            <b>Oldalak száma:<br /></b> {currentBook.pages}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default BookModal