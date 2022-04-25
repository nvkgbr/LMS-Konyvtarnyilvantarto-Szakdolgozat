import React from 'react';
import '../index.css';
import { useState, useEffect } from 'react';
import axios from 'axios';
import BookModal from '../components/BookModal';

function Books() {
    const [books, setBooks] = useState([]);
    const [openModal, setOpenModal] = useState(false);
    const [currentBook, setCurrentBook] = useState({});

    useEffect(() => {
        let bookList;
        (async () => {
            try {
                const response = await axios.get("https://localhost:5001/api/book");
                bookList = await response.data;
                for (let i = 0; i < bookList.length; i++) {
                    bookList[i].author = await GetAuthorsName(bookList[i].id);
                }
            }
            catch (err) {
                console.log(err);
            }
            finally {
                setBooks(bookList);
            }
        })();
    }, []);
    
    async function GetAuthorsName(bookId) {
        const authorList = await axios.get("https://localhost:5001/api/Book/GetAuthors/" + bookId);
        let author = "";

        for await (const instance of authorList.data) {
            author += instance + ", ";
        }
        return author.slice(0, -2);
    }

    function SortBooksByName(way) {
        if (way) {
            books.sort(function (a, b) {
                var textA = a.title.toUpperCase();
                var textB = b.title.toUpperCase();
                return textA.localeCompare(textB);
            });
        } else {
            books.sort(function (a, b) {
                var textA = a.title.toUpperCase();
                var textB = b.title.toUpperCase();
                return textB.localeCompare(textA);
            });
        }
        setBooks([...books]);
    }

    function SortBooksByReleaseDate(way) {
        if (way) {
            books.sort((a, b) => {
                return a.publishYear - b.publishYear;
            });
        } else {
            books.sort((a, b) => {
                return a.publishYear - b.publishYear;
            }).reverse();
        }
        setBooks([...books]);
    }

    return (
        <div style={{paddingTop:50}} >
             {openModal && <BookModal closeModal={setOpenModal} currentBook={currentBook} />}

            <div className="container-fluid">
                <p></p>

                <div className='justify-content-center d-flex'>

                    <p className="sortTitlep">cím</p>
                    <div className="sort_buttons bookTitle">
                        <a onClick={() => SortBooksByName(true)}><svg width="16px" height="16px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg"><path d="M 16 8.59375 L 15.28125 9.28125 L 5.28125 19.28125 L 3.59375 21 L 28.40625 21 L 26.71875 19.28125 L 16.71875 9.28125 Z M 16 11.4375 L 23.5625 19 L 8.4375 19 Z" /></svg></a>
                        <a onClick={() => SortBooksByName(false)}><svg width="16px" height="16px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg"><path d="M 3.59375 12 L 5.28125 13.71875 L 15.28125 23.71875 L 16 24.40625 L 16.71875 23.71875 L 26.71875 13.71875 L 28.40625 12 Z M 8.4375 14 L 23.5625 14 L 16 21.5625 Z" /></svg> </a>
                    </div>

                    <p className="sortTitlep">kiadási év</p>
                    <div className="sort_buttons publishYear">
                        <a onClick={() => SortBooksByReleaseDate(true)}><svg width="16px" height="16px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg"><path d="M 16 8.59375 L 15.28125 9.28125 L 5.28125 19.28125 L 3.59375 21 L 28.40625 21 L 26.71875 19.28125 L 16.71875 9.28125 Z M 16 11.4375 L 23.5625 19 L 8.4375 19 Z" /></svg></a>
                        <a onClick={() => SortBooksByReleaseDate(false)}><svg width="16px" height="16px" viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg"><path d="M 3.59375 12 L 5.28125 13.71875 L 15.28125 23.71875 L 16 24.40625 L 16.71875 23.71875 L 26.71875 13.71875 L 28.40625 12 Z M 8.4375 14 L 23.5625 14 L 16 21.5625 Z" /></svg> </a>
                    </div>

                </div>

                <div className="row pb-5">
                    {console.log(books)}
                    {books.map((book) => (
                        <div className="col-xs-12 col-sm-8 col-md-6 col-lg-4 col-xl-3 pt-5" key={book.id}>
                            <div style={{ cursor: 'pointer' }} onClick={() => { setCurrentBook(book); setOpenModal(true) }}>
                                <div className='bookCard' style={{
                                    background: `url("https://localhost:5001/Img/${book.link}")`,
                                    backgroundRepeat: "no-repeat",
                                    backgroundSize: "250px 350px",
                                }}>

                                    <div className='overlay text-center'>
                                        <div className='items'></div>
                                        <div className='items head'>
                                            <h6 className='items'>{book.author}</h6>
                                            <h5 className='head'>{book.title}</h5>
                                        </div>
                                        <h5 className='items'>{book.category}</h5>
                                        <div>{book.publishYear}</div>
                                        <div>{book.publisher}</div>
                                        <div>{book.isbn}</div>
                                        <div>{book.pages} oldal</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default Books