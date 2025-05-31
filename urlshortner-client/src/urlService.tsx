import axios from 'axios';

const axiosInstance  = axios.create({
     baseURL :import.meta.env.VITE_API_URL,
    headers:{
        "Content-Type":"application/json"
    }
});

//
export const sortenUrl= async (longUrl: string)=>{
    const response= await axiosInstance.post('shorten',{
     LongUrl:longUrl
    });
    return response?.data;
}
