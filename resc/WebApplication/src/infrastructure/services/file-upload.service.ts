import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import { AttachedFile } from '../models/attached-file.model';

@Injectable()
export class FileUploadService {
  public uploadFile(url: string, file: File): Observable<AttachedFile> {
    return new Observable((observer: Observer<AttachedFile>) => {
      const formData = new FormData();
      const xhr = new XMLHttpRequest();

      formData.append('file', file);
      xhr.open('POST', url, true);
      xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
      xhr.send(formData);

      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4) {
          if (xhr.status === 200) {
            observer.next(JSON.parse(xhr.response));
            observer.complete();
          } else {
            observer.error(xhr.response);
          }
        }
      };
    });
  }
}
