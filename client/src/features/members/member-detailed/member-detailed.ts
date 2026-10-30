import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { AccountService } from '../../../core/services/account-service';
import { PresenceService } from '../../../core/services/presence-service';
import { LikesService } from '../../../core/services/likes-service';
import { Location } from '@angular/common';
@Component({
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  selector: 'app-member-detailed',
  styleUrl: './member-detailed.css',
  templateUrl: './member-detailed.html',
})
export class MemberDetailed implements OnInit {

  protected memberService = inject(MemberService);
  private accountService = inject(AccountService);
  protected presenceService = inject(PresenceService);
  protected likesService = inject(LikesService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  protected title = signal<string | undefined>('Profile');
  private routeId = signal<string | null>(null);
  protected isCurrentUser = computed(() => {
    return this.accountService.currentUser()?.id === this.route.snapshot.paramMap.get('id');
  });

  protected hasLiked = computed(() => this.likesService.likeIds().includes(this.routeId()!))
  constructor(private location: Location) {
    this.route.paramMap.subscribe(params => {
      this.routeId.set(params.get('id'));
    })
  }
  goBack() {
    this.location.back();
  }

  ngOnInit(): void {

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
