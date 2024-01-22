import { Component, OnInit } from '@angular/core';
import { Client } from '../../models/client';
import { ClientService } from '../../services/client.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-viewClients',
  templateUrl: './viewClients.component.html',
  styleUrls: ['./viewClients.component.css']
})
export class ViewClientsComponent implements OnInit {
  clients: Client[] = []
  clientsSearch: Client[] = []
  selectedRows: Set<string> = new Set<string>();

  constructor(
    private clientService: ClientService,
    private router: Router
  ) { }

  ngOnInit() {
    this.clientService.getAll().subscribe({
      next: (data: Client[]) => {
        this.clients = data;
        this.clientsSearch = data;
      },
      error: (error) => {
        console.error('Erro ao obter dados de clientes:', error);
      }
    });
  }

toggleRowSelection(index: string): void {
    if (this.selectedRows.has(index)) {
        this.selectedRows.delete(index);
    } else {
        this.selectedRows.add(index);
    }
}

  deleteSelectedRows(): void {
    if(Array.from(this.selectedRows).length > 0){
      for (let index = 0; index < Array.from(this.selectedRows).length; index++) {
        this.clientService.delete(Array.from(this.selectedRows)[index]).subscribe({
          next: () => {
            this.clients = this.clients.filter(d => d.id != Array.from(this.selectedRows)[index])
            this.clientsSearch = this.clients.filter(d => d.id != Array.from(this.selectedRows)[index])
          }
        })
      }
    }else {
      alert("Selecione um pelo menos, para excluir.")
    }
  }

  searchValues(event: any) {
    const values = (event.target as HTMLInputElement).value;

    if(!values){
      this.clientsSearch = this.clients
      return
    }

    this.clientsSearch = this.clients.filter(x =>
      x.name?.toLowerCase().includes(values?.toLowerCase()) ||
      x.cpf?.toLowerCase().includes(values?.toLowerCase()) ||
      x.email?.toLowerCase().includes(values?.toLowerCase()) ||
      x.emailUser?.toLowerCase().includes(values?.toLowerCase())
    );
  }

}
