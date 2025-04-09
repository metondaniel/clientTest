import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ClientsService } from '../../services/client.service';
import { Client } from '../../client.model';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.scss']
})
export class ClientFormComponent {
  form: FormGroup;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private service: ClientsService,
    private dialogRef: MatDialogRef<ClientFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: Client
  ) {
    this.form = this.fb.group({
      nomeRazaoSocial: ['', Validators.required],
      cpfCnpj: ['', [Validators.required, cpfCnpjValidator()]],
      email: ['', [Validators.required, Validators.email]],
      tipo: ['FISICA', Validators.required],
      dataNascimento: [],
      telefone: [],
      endereco: this.fb.group({
        cep: ['', Validators.required],
        logradouro: ['', Validators.required],
        numero: ['', Validators.required],
        bairro: ['', Validators.required],
        cidade: ['', Validators.required],
        estado: ['', Validators.required]
      }),
      inscricaoEstadual: [],
      isentoIE: [false]
    });

    this.form.get('tipo')?.valueChanges.subscribe(tipo => {
      this.updateValidations(tipo);
    });
    if (data) {
      this.isEdit = true;
      this.form.patchValue(data);
    }
  }

  private updateValidations(tipo: string) {
    const ctrlDataNascimento = this.form.get('dataNascimento');
    const ctrlInscricaoEstadual = this.form.get('inscricaoEstadual');
    const ctrlIsentoIE = this.form.get('isentoIE');

    if (tipo === 'FISICA') {
      ctrlDataNascimento?.setValidators([Validators.required]);
      ctrlInscricaoEstadual?.clearValidators();
      ctrlIsentoIE?.clearValidators();
    } else {
      ctrlDataNascimento?.clearValidators();
      ctrlInscricaoEstadual?.setValidators([Validators.required]);
      ctrlIsentoIE?.setValidators([Validators.required]);
    }

    ctrlDataNascimento?.updateValueAndValidity();
    ctrlInscricaoEstadual?.updateValueAndValidity();
    ctrlIsentoIE?.updateValueAndValidity();
  }

  onSubmit() {
    if (this.form.valid) {
      const operation = this.isEdit
        ? this.service.updateClient(this.data!.id, this.form.value)
        : this.service.createClient(this.form.value);

      operation.subscribe(() => this.dialogRef.close(true));
    }
  }
}
function cpfCnpjValidator(): any | string {

}

