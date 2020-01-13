<template>
  <v-dialog
    v-model="dialog"
    max-width="500px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.congregation.create'"
      />
    </template>
    <v-card>
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-card-title v-t="'project.congregation.create'" />
        <v-card-text>

          <p
            class="subtitle-1"
            v-t="'project.congregation.createDescription'"
          />

          <p v-t="'project.congregation.nameDescription'" />
          <v-text-field
            v-model="name"
            :label="$t('core.name')"
            :rules="nameRules"
            filled
            required
          />

          <p v-t="'project.congregation.numberDescription'" />
          <v-text-field
            v-model="number"
            :label="$t('project.congregation.number')"
            :rules="numberRules"
            filled
            required
          />

        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            v-t="'core.cancel'"
            @click.stop="dialog = false"
          />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            @click.stop="create"
            :disabled="!valid"
            :loading="loading"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import AlertModule from '../../store/alerts'
import CongregationModule from '../../store/congregations'
import { Topic, Exceptions } from '../../models'

@Component
export default class CreateCongregationDialog extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
  private congregationModule: CongregationModule = getModule(CongregationModule, this.$store)

  private dialog: any = null
  private valid: boolean = false
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 })
  ]
  private number: string = ''
  private numberRules: any[] = [
    (v: string) => v.length <= 10 || i18n.t('core.fieldMax', { count: 10 }),
  ]

  private async create() {
    this.loading = true

    const response = await this.congregationModule.createCongregation({
      name: this.name,
      number: this.number,
    })
    if (response === Exceptions.CongregationNameUnique) {
      const form: any = this.$refs.form
      const thisName = this.name
      this.nameRules.push((v: string) => v !== thisName || i18n.t('project.congregation.nameUnique'))
      form.validate()
    } else if (response === Exceptions.CongregationNumberUnique) {
      const form: any = this.$refs.form
      const thisNumber = this.number
      this.numberRules.push((v: string) => v !== thisNumber || i18n.t('project.congregation.numberUnique'))
      form.validate()
    } else if (response) {
      this.dialog = false
    } else {
      this.alertModule.showAlert({
        message: i18n.t('core.generalError').toString(),
        color: 'error',
        timeout: 5000,
      })
    }

    this.loading = false
  }

}
</script>
