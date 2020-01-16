<template>
  <div>
    <p v-t="translationPath" />
    <v-text-field
      v-model="model.value"
      :label="$t('core.name')"
      :rules="nameRules"
      filled
      required
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'

@Component
export default class NameControl extends Vue {

  @Prop(Object)
  private readonly model: { value: string } = { value: '' }

  @Prop(String)
  private readonly translationPath: string = ''

  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length >= 2 || i18n.t('core.fieldMin', { count: 2 }),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
  ]

}
</script>
