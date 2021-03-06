import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FilesService {
  private uriseg = 'https://localhost:7158/api/files';

  constructor(private http: HttpClient) { }

  private getAuthHeader() : HttpHeaders {
    const headers= new HttpHeaders()
    .set('Authorization', 'Bearer ' + localStorage.getItem('auth_tkn'))
    .set('Access-Control-Allow-Origin', '*');
    return headers;
  }

  public getAllFiles(): Observable<any> {
    const URI = this.uriseg;
    let headers = this.getAuthHeader();
    return this.http.get(URI, { 'headers': headers });
  }

  public edit(file: File) : Observable<any> {
    const URI = this.uriseg;
    let headers = this.getAuthHeader();
    const formData = new FormData();
    formData.append('', file);
    return this.http.put(URI, formData, { 'headers': headers });
  }

  public delete(fileName: string) : Observable<any> {

    const URI = this.uriseg + '/' + fileName;
    let headers = this.getAuthHeader();
    return this.http.delete(URI, { 'headers': headers });
  }

  public download(fileName: string) : Observable<Blob> {

    const URI = this.uriseg + '/download/' + fileName;
    let headers = this.getAuthHeader();
    return this.http.get(URI, { 'headers': headers,  responseType: 'blob' });
  }

  public upload(file: File) : Observable<any> {
    const URI = this.uriseg;
    let headers = this.getAuthHeader();
    const formData = new FormData();
    formData.append('', file);
    return this.http.post(URI, formData, { 'headers': headers });
  }

  public sizeToString(size){
    const units = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
     let l = 0, n = parseInt(size, 10) || 0;
     while(n >= 1024 && ++l){
         n = n/1024;
     }
     return(n.toFixed(n < 10 && l > 0 ? 1 : 0) + ' ' + units[l]);
   }
}
