import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age-pipe';

@Component({
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  selector: 'app-member-detailed',
  styleUrl: './member-detailed.css',
  templateUrl: './member-detailed.html',
})
export class MemberDetailed implements OnInit {

  private memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  protected member = signal<Member | undefined>(undefined);
  protected title = signal<string | undefined>('Profile');

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member.set(data['member'])
    })
    this.title.set(this.route.firstChild?.snapshot?.title)

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title)
      }
    })
  }

  loadMember() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    return this.memberService.getMember(id);

  }

}
