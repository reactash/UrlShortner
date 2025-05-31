import axios from 'axios';

const axiosInstance  = axios.create({
     baseURL :'/api/v1.0/url',
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
