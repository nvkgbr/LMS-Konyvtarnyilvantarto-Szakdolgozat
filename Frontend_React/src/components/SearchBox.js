import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function SearchBox() {
    const navigate = useNavigate();

    const [query, setQuery] = useState();

    return (
        <div className="search-box">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="black" className="bi bi-search"
                viewBox="0 0 16 16">
                <path
                    d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
            </svg>
            <input id="searchBox" type="text" className="search-input" placeholder="Keresés..." onKeyPress={(e) => {
                if (e.key === "Enter") 
                { 
                    if(e.target.value.toString().length > 0){
                        setQuery(e.target.value.toString()); 
                        navigate("/search/:" + query); 
                        window.location.reload(); 
                    }
                }
            }} onChange={(inpt) => {
                setQuery(inpt.target.value.toString());
            }} ></input>
            <a className="search-btn" href={"/search/:" + query}>
            </a>
        </div>
    )
}
