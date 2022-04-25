import React from 'react'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import axios from 'axios';
import BookModal from '../components/BookModal';

export default function Search() {
  let { query } = useParams();
  const [books, setBooks] = useState([]);
  const [openModal, setOpenModal] = useState(false);
  const [currentBook, setCurrentBook] = useState({});

  useEffect(() => {
    let bookList;
    let filtered = [];
    (async () => {
      try {
        const response = await axios.get("https://localhost:5001/api/book");
        bookList = await response.data;
        for (let i = 0; i < bookList.length; i++) {
          bookList[i].author = await GetAuthorsName(bookList[i].id);
          console.log(query.substring(1));
          if ((bookList[i].title.toLowerCase().indexOf(query.substring(1).toLowerCase()) !== -1) ||
            (bookList[i].author.toLowerCase().indexOf(query.substring(1).toLowerCase()) !== -1)) { filtered.push(bookList[i]); }
        }
      }
      catch (err) {
        console.log(err);
      }
      finally {
        console.log(filtered);
        setBooks(filtered);
      }
    })();
  }, [query]);

  async function GetAuthorsName(bookId) {
    const authorList = await axios.get("https://localhost:5001/api/Book/GetAuthors/" + bookId);
    let author = "";

    for await (const instance of authorList.data) {
      author += instance + ", ";
    }
    return author.slice(0, -2);
  }


  return (
    <div>
      {openModal && <BookModal closeModal={setOpenModal} currentBook={currentBook} />}

      <div className="container-fluid">
        <div className="row pb-5 pt-5">
          {books.map((book) => (
            <div className="col-xs-12 col-sm-8 col-md-6 col-lg-4 col-xl-3 pt-5">
              <div style={{ cursor: 'pointer' }} onClick={() => { setCurrentBook(book); setOpenModal(true) }}>
                <div className='bookCard' style={{
                  background: `url("https://localhost:5001/Img/${book.link}")`,
                  backgroundRepeat: "no-repeat",
                  backgroundSize: "250px 350px",
                  fontWeight: "bold"
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

