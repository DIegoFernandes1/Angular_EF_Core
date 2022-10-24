import { Evento } from "./Evento";

export interface Lote {
  id: number;
  nome: string;
  preco: number;
  dataInicio?: Date;
  dataim: Date;
  quantidade: number;
  idEvento?: number;
  evento: Evento;
}
