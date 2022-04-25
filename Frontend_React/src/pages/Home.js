import React from 'react'
import { useNavigate } from 'react-router-dom';

function Home() {
  const navigate = useNavigate(); 
  const routeChange = (path) =>{  
    navigate(path);
  }

  return (
    <div className='conatiner-fluid headers'>
      <div style={{ backgroundImage: "url(./img/headerv2-v.jpg)", backgroundSize: "cover", opacity: .9 }} className="conatiner-fluid home-dsng-top vertical-align-middle" >
        <br />
        <h3>Üdvözöljük az oldalunkon! </h3> <br /><br />
        <p>
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. <br />
          Nam fermentum rhoncus gravida. Cras id maximus ante. <br />
          Morbi lacinia sit amet dui sit amet malesuada. Nam in tempor arcu.
        </p>
        <button className="home-btn-top" onClick={()=> routeChange("books")}>Böngészés a könyvek között!</button> <br /> <br />
      </div>
      <div style={{ backgroundImage: "url(./img/header_v4.png)", backgroundSize: "cover", opacity: .9 }} className="conatiner-fluid home-dsng-bottom" >
        <br />
        <h3>Hosszabbítsa meg kölcsönzését otthonról! </h3> <br /> <br />
        <p>
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. <br />
          Nam fermentum rhoncus gravida. Cras id maximus ante. <br />
          Morbi lacinia sit amet dui sit amet malesuada. Nam in tempor arcu.
        </p>
        <button className="home-btn-bottom" onClick={()=> routeChange("login")}>Jelentkezzen be!</button> <br /> <br />
      </div>
    </div>
  )
}

export default Home