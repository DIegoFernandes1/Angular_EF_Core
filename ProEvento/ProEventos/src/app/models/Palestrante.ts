import { RedeSocial } from "./RedeSocial"

export interface Palestrante {
  id: number;
  nome: string;
  descricao: string;
  imagemURL: string;
  telefone: string;
  email: string;
  redeSocials: RedeSocial[];
  palestrantesEventos: Palestrante[];
}
