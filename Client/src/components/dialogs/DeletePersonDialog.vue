<template>
  <v-dialog
    v-model="deletePersonDialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        color="error"
        v-t="'person.delete'"
        @click="opened"
      />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span
            class="headline"
            v-t="'person.delete'"
          />
        </v-card-title>
        <v-card-text>
          <p
            class="subtitle-1"
            v-t="'person.deleteDescription'"
          />
          <p
            class="subtitle-1"
            v-t="'person.deleteHint'"
          />
          <v-text-field
            v-model="enteredName"
            :label="$t('core.name')"
            :rules="enteredNameRules"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="deletePersonDialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="error"
            @click.stop="deletePerson"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import PersonModule from '../../store/persons'
import { Person } from '../../models'

@Component
export default class DeletePersonDialog extends Vue {
  private personModule = getModule(PersonModule, this.$store)

  @Prop(String)
  private readonly personId?: string

  private valid: any = false
  private deletePersonDialog = false
  private personFirstname: string = ''
  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.personFirstname || i18n.t('person.nameMustEqual'),
    ]
  }

  private async opened() {
    if (this.personId) {
      const person = this.personModule.getPersons.find((x: Person) => x.id === this.personId)
      if (person) {
        this.personFirstname = person.firstname
      }
    }
  }

  private async deletePerson() {
    if (this.personId) {
      await this.personModule.deletePerson(this.personId)

      this.deletePersonDialog = false

      this.$router.push({ name: 'account' })
    }
  }

}
</script>
