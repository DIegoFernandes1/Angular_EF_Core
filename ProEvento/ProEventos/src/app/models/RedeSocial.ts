import { Evento } from "./Evento";
import { Palestrante } from "./Palestrante";

export interface RedeSocial {
  id: number;
  nome: string;
  URL: string;
  idEvento?: number;
  evento: Evento;
  idPalestrante?: number;
  palestrante: Palestrante;
}
