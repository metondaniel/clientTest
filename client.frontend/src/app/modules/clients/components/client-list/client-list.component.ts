import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ClientFormComponent } from '../client-form/client-form.component';
import { ClientsService } from '../../services/client.service';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { Client } from '../../client.model';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.scss']
})
export class ClientListComponent implements OnInit {
  clients: Client[] = [];
  displayedColumns = ['nome', 'documento', 'tipo', 'email', 'actions'];

  constructor(
    private service: ClientsService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loadClients();
  }

  openForm(client?: Client) {
    const dialogRef = this.dialog.open(ClientFormComponent, {
      width: '600px',
      data: client
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadClients();
    });
  }

  deleteClient(id: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Confirmação', message: 'Deseja realmente excluir este cliente?' }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.service.deleteClient(id).subscribe(() => this.loadClients());
      }
    });
  }

  private loadClients() {
    this.service.getClients().subscribe(clients => this.clients = clients);
  }
}
