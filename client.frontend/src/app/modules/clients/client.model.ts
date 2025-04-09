export interface Client {
  id: string;
  nomeRazaoSocial: string;
  cpfCnpj: string;
  email: string;
  tipo: 'FISICA' | 'JURIDICA';
  dataNascimento?: Date;
  telefone?: string;
  endereco: {
    cep: string;
    logradouro: string;
    numero: string;
    bairro: string;
    cidade: string;
    estado: string;
  };
  inscricaoEstadual?: string;
  isentoIE: boolean;
  dataCadastro: Date;
}
