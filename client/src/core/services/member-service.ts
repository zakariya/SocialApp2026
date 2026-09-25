import { inject, Service } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member, Photo } from '../../types/member';

@Service()
export class MemberService {
    private baseUrl = environment.apiUrl;
    private http = inject(HttpClient);


    getMembers() {
        return this.http.get<Member[]>(this.baseUrl + 'members');
    }
    getMember(id: string) {
        return this.http.get<Member>(this.baseUrl + 'members/' + id);

    }

    getMemberPhotos(id: string) {
        return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + "/photos");

    }


}
