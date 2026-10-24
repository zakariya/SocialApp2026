import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { UserManagement } from './user-management/user-management';
import { PhotoManagement } from './photo-management/photo-management';

@Component({
  imports: [UserManagement, PhotoManagement],
  selector: 'app-admin',
  styleUrl: './admin.css',
  templateUrl: './admin.html',
})
export class Admin {
  protected accountService = inject(AccountService);
  activeTab = 'photos';
  tabs = [
    { label: 'Photo moderation', value: 'photos' },
    { label: 'User management', value: 'roles' },

  ]

  setTab(tab: string) {
    this.activeTab = tab;
  }

}
