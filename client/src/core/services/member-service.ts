import { inject, Service, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { EditableMember, Member, Photo } from '../../types/member';
import { tap } from 'rxjs';

@Service()
export class MemberService {
    private baseUrl = environment.apiUrl;
    private http = inject(HttpClient);
    public editMode = signal(false);
    public member = signal<Member | null>(null);


    getMembers() {
        return this.http.get<Member[]>(this.baseUrl + 'members');
    }
    getMember(id: string) {
        return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
            tap(member => {
                this.member.set(member)
            })
        );

    }

    getMemberPhotos(id: string) {
        return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + "/photos");

    }
    updateMember(member: EditableMember) {
        return this.http.put(this.baseUrl + 'members', member);
    }


}
