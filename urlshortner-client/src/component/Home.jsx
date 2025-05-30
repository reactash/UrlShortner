import React, { useState } from "react";
import { sortenUrl } from "../urlService";

const Home = () => {
  const [sortUrl, setSortUrl] = useState("");
  const [longUrl, setLongUrl] = useState("");
  const [error, setError] = useState("");

  const handleCopyUrl=async()=>{
    
    if(sortUrl=='')
    {
      return;
    }
    try{
      await navigator.clipboard.writeText(sortUrl);
      alert('url copied');

    }catch (err)
    {
      console.log('Failed to copy');
    }
  }

  const handleSortenUrl = async () => {
    setSortUrl("");
    setError("");

    if (!longUrl.trim()) {
      setError("Please enter a valid url");
      return;
    }

    try {
      const result = await sortenUrl(longUrl);
      console.log(result);
      if (typeof result === "string") {
        setSortUrl(result);
         setLongUrl('');
      } else {
        setError("Unexpected response");
      }
     
    } catch (err) {
      console.log(err);
      const msg = err.response?.data?.message || "Failed to shorten URL";
      setError(msg);
    }
  };

  return (
    <>
      <div className="home">
        <h1 className="heading">Short URL</h1>
        <div className="search">
           <div className="input-group">        
            <input
              type="text"
              className="search-bar"
              placeholder="Enter the link here"
              value={longUrl}
              onChange={(e) => setLongUrl(e.target.value)}
            ></input>
            <button className="btn" onClick={handleSortenUrl}>
              Shorten URL
            </button>
          </div> 
          <p className="description">
            ShortURL is a free tool to shorten URLs and generate short links URL
            shortener allows to create a shortened link making it easy to share
          </p>
        </div>

        <div className="shorten-url">
          <p className="shorturl-text">{sortUrl &&<span>{sortUrl}</span>}
          {error && <span style={{color:'red'}}>{error}</span>}
          </p>
          <button className="copy-url" onClick={handleCopyUrl}>Copy Url</button>
        </div>
        <footer>Created by @ashweets</footer>
      </div>
    </>
  );
};

export default Home;
