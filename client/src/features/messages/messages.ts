import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from '../../core/services/message-service';
import { PaginatedResult } from '../../types/Pagination';
import { Message } from '../../types/message';
import { Paginator } from '../../shared/paginator/paginator';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { ConfirmDialogService } from '../../core/services/confirm-dialog-service';

@Component({
  imports: [Paginator, RouterLink, DatePipe],
  selector: 'app-messages',
  styleUrl: './messages.css',
  templateUrl: './messages.html',
})
export class Messages implements OnInit {


  private messageService = inject(MessageService);
  private confirmDialog = inject(ConfirmDialogService);
  protected container = 'Inbox';
  protected fetchedContainer = 'Inbox';
  protected pageNumber = 1;
  protected pageSize = 10;
  protected paginatedMessage = signal<PaginatedResult<Message> | null>(null);
  tabs = [
    { label: 'Inbox', value: 'Inbox' },
    { label: 'Outbox', value: 'Outbox' }
  ]

  ngOnInit(): void {
    this.loadMessage();
  }

  loadMessage() {
    this.messageService.getMessage(this.container, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.paginatedMessage.set(response);
        this.fetchedContainer = this.container;
      }
    })
  }

  async confirmDelete(event: Event, id: string) {
    event.stopPropagation();
    const ok = await this.confirmDialog.confirm('Are you sure want to delete this message?');
    if (ok) this.deleteMessage(id);
  }

  deleteMessage(id: string) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => {
        const current = this.paginatedMessage();
        if (current?.items) {
          this.paginatedMessage.update(prev => {
            if (!prev) return null;

            const newItems = prev.items.filter(x => x.id !== id) || [];

            return {
              items: newItems,
              metaData: prev.metaData
            }

          })
        }
      }
    })

  }

  get isInbox() {
    return this.fetchedContainer === "Inbox";
  }
  setContainer(container: string) {
    this.container = container;
    this.pageNumber = 1;
    this.loadMessage();
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    this.loadMessage();
  }


}
